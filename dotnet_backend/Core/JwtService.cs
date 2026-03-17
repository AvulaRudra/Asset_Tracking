using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dotnet_backend.Core;

public class JwtService
{
    private readonly string _secret;
    private readonly int _expiresMinutes;

    public JwtService(IConfiguration config)
    {
        _secret = config["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret missing");
        _expiresMinutes = int.Parse(config["Jwt:ExpiresMinutes"] ?? "60");
    }

    public string CreateAccessToken(string subject, string provider, string? email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, subject),
            new Claim("provider", provider),
            new Claim(ClaimTypes.Email, email ?? "")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expiresMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

