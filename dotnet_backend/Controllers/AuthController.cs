using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Google;
using dotnet_backend.Services;
using dotnet_backend.DTOs;
using System.Security.Claims;
namespace dotnet_backend.Controllers;

[ApiController]
[Route("auth")]
[Tags("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _config;
    private readonly string _frontendBaseUrl;

    public AuthController(IAuthService authService, IConfiguration config)
    {
        _authService = authService;
        _config = config;
        _frontendBaseUrl = _config["FrontendBaseUrl"] ?? "http://localhost:4200";
    }

    [HttpGet("google/login")]
    public IActionResult GoogleLogin()
    {
        var redirectUri = Url.Action(nameof(GoogleCallback))!;
        var properties = new AuthenticationProperties { RedirectUri = redirectUri };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return BadRequest("Google auth failed");

        var subject = result.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                      result.Principal?.FindFirst("sub")?.Value ?? throw new InvalidOperationException("No subject");
        var email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value;

        var token = _authService.HandleSsoLogin("google", subject, email);
        var redirectTo = $"{_frontendBaseUrl}/login?token={token}";
        return Redirect(redirectTo);
    }
    // Similar for Azure - configure in Program.cs if needed
    [HttpGet("azure/login")]
    public IActionResult AzureLogin()
    {
        var redirectUri = Url.Action(nameof(AzureCallback))!;
        var properties = new AuthenticationProperties { RedirectUri = redirectUri };
        return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet("azure/callback")]
    public async Task<IActionResult> AzureCallback()
    {
        var result = await HttpContext.AuthenticateAsync(OpenIdConnectDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return BadRequest("Azure auth failed");

        var subject = result.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                      result.Principal?.FindFirst("oid")?.Value ?? throw new InvalidOperationException("No subject");
        var email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value ?? 
                    result.Principal?.FindFirst("preferred_username")?.Value;

        var token = _authService.HandleSsoLogin("azure", subject, email);
        var redirectTo = $"{_frontendBaseUrl}/login?token={token}";
        return Redirect(redirectTo);
    }
}

