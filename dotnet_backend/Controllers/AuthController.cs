// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Authentication.OpenIdConnect;
// using Microsoft.AspNetCore.Authentication.Google;
// using dotnet_backend.Services;
// using dotnet_backend.DTOs;
// using System.Security.Claims;
// namespace dotnet_backend.Controllers;

// [ApiController]
// [Route("auth")]
// [Tags("auth")]
// public class AuthController : ControllerBase
// {
//     private readonly IAuthService _authService;
//     private readonly IConfiguration _config;
//     private readonly string _frontendBaseUrl;

//     public AuthController(IAuthService authService, IConfiguration config)
//     {
//         _authService = authService;
//         _config = config;
//         _frontendBaseUrl = _config["FrontendBaseUrl"] ?? "http://localhost:4200";
//     }

//     [HttpGet("google/login")]
// public IActionResult GoogleLogin()
// {
//     // Explicitly set the redirect URI so it always resolves correctly
//     var redirectUri = $"{Request.Scheme}://{Request.Host}/auth/google/callback";
//     var properties = new AuthenticationProperties { RedirectUri = redirectUri };
//     return Challenge(properties, GoogleDefaults.AuthenticationScheme);
// }

//     [HttpGet("google/callback")]
//     public async Task<IActionResult> GoogleCallback()
//     {
//         var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
//         if (!result.Succeeded)
//             return BadRequest("Google auth failed");

//         var subject = result.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
//                       result.Principal?.FindFirst("sub")?.Value ?? throw new InvalidOperationException("No subject");
//         var email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value;

//         var token = _authService.HandleSsoLogin("google", subject, email);
//         var redirectTo = $"{_frontendBaseUrl}/login?token={token}";
//         return Redirect(redirectTo);
//     }
//     [HttpGet("error")]
// public IActionResult Error([FromQuery] string msg)
// {
//     return BadRequest(new { error = msg });
// }


//     // Azure AD Login - Commented out
//     /*
//     [HttpGet("azure/login")]
//     public IActionResult AzureLogin()
//     {
//         var redirectUri = Url.Action(nameof(AzureCallback))!;
//         var properties = new AuthenticationProperties { RedirectUri = redirectUri };
//         return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
//     }

//     [HttpGet("azure/callback")]
//     public async Task<IActionResult> AzureCallback()
//     {
//         var result = await HttpContext.AuthenticateAsync(OpenIdConnectDefaults.AuthenticationScheme);
//         if (!result.Succeeded)
//             return BadRequest("Azure auth failed");

//         var subject = result.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
//                       result.Principal?.FindFirst("oid")?.Value ?? throw new InvalidOperationException("No subject");
//         var email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value ?? 
//                     result.Principal?.FindFirst("preferred_username")?.Value;

//         var token = _authService.HandleSsoLogin("azure", subject, email);
//         var redirectTo = $"{_frontendBaseUrl}/login?token={token}";
//         return Redirect(redirectTo);
//     }
//     */
// }

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using dotnet_backend.Services;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace dotnet_backend.Controllers;

[ApiController]
[Route("auth")]
[Tags("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _config;
    private readonly string _frontendBaseUrl;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _backendBaseUrl;

    public AuthController(IAuthService authService, IConfiguration config)
    {
        _authService     = authService;
        _config          = config;
        _frontendBaseUrl = _config["FrontendBaseUrl"] ?? "https://localhost:4200";
        _backendBaseUrl  = _config["BackendBaseUrl"]  ?? "https://localhost:5000";
        _clientId        = _config["Google:ClientId"] ?? "";
        _clientSecret    = _config["Google:ClientSecret"] ?? "";
    }

    // ── Step 1: Redirect to Google ────────────────────────────────────────────
    [HttpGet("google/login")]
    public IActionResult GoogleLogin()
    {
        // Generate a random state value and store it in session
        var state = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                           .Replace("+", "-").Replace("/", "_").Replace("=", "");
        HttpContext.Session.SetString("oauth_state", state);

        var callbackUrl = $"{_backendBaseUrl}/auth/google/callback";

        var googleAuthUrl = QueryHelpers.AddQueryString(
            "https://accounts.google.com/o/oauth2/v2/auth",
            new Dictionary<string, string?>
            {
                ["client_id"]     = _clientId,
                ["redirect_uri"]  = callbackUrl,
                ["response_type"] = "code",
                ["scope"]         = "openid email profile",
                ["state"]         = state,
                ["access_type"]   = "offline",
                ["prompt"]        = "select_account"
            });

        return Redirect(googleAuthUrl);
    }

    // ── Step 2: Google redirects here with ?code=...&state=... ────────────────
    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback([FromQuery] string code,
                                                    [FromQuery] string state)
    {
        // Validate state to prevent CSRF
        var savedState = HttpContext.Session.GetString("oauth_state");
        if (string.IsNullOrEmpty(savedState) || savedState != state)
            return Redirect($"{_frontendBaseUrl}/login?error=invalid_state");

        HttpContext.Session.Remove("oauth_state");

        // Exchange code for tokens
        var callbackUrl = $"{_backendBaseUrl}/auth/google/callback";
        var tokenResponse = await ExchangeCodeAsync(code, callbackUrl);
        if (tokenResponse is null)
            return Redirect($"{_frontendBaseUrl}/login?error=token_exchange_failed");

        // Get user info from Google
        var userInfo = await GetUserInfoAsync(tokenResponse.AccessToken);
        if (userInfo is null)
            return Redirect($"{_frontendBaseUrl}/login?error=userinfo_failed");

        // Upsert user + issue JWT
        var jwt = _authService.HandleSsoLogin("google", userInfo.Sub, userInfo.Email);
        return Redirect($"{_frontendBaseUrl}/login?token={jwt}");
    }

    [HttpGet("error")]
    public IActionResult Error([FromQuery] string msg) =>
        BadRequest(new { error = msg });

    // ── Helpers ───────────────────────────────────────────────────────────────
    private async Task<GoogleTokenResponse?> ExchangeCodeAsync(string code, string redirectUri)
    {
        using var http = new HttpClient();
        var resp = await http.PostAsync("https://oauth2.googleapis.com/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["code"]          = code,
                ["client_id"]     = _clientId,
                ["client_secret"] = _clientSecret,
                ["redirect_uri"]  = redirectUri,
                ["grant_type"]    = "authorization_code"
            }));

        if (!resp.IsSuccessStatusCode) return null;
        var json = await resp.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GoogleTokenResponse>(json);
    }

    private async Task<GoogleUserInfo?> GetUserInfoAsync(string accessToken)
    {
        using var http = new HttpClient();
        http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        var resp = await http.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
        if (!resp.IsSuccessStatusCode) return null;
        var json = await resp.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GoogleUserInfo>(json);
    }

    private record GoogleTokenResponse(
        [property: System.Text.Json.Serialization.JsonPropertyName("access_token")]  string AccessToken,
        [property: System.Text.Json.Serialization.JsonPropertyName("id_token")]       string? IdToken,
        [property: System.Text.Json.Serialization.JsonPropertyName("refresh_token")]  string? RefreshToken
    );

    private record GoogleUserInfo(
        [property: System.Text.Json.Serialization.JsonPropertyName("sub")]   string Sub,
        [property: System.Text.Json.Serialization.JsonPropertyName("email")] string? Email
    );
}