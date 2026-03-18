using System.Security.Cryptography;
using dotnet_backend.Repositories;
using dotnet_backend.Core;

namespace dotnet_backend.Services;

public class LocalAuthService : ILocalAuthService
{
    private readonly ILocalUserRepository _repo;
    private readonly PasswordHasher _passwordHasher;
    private readonly JwtService _jwtService;

    public LocalAuthService(ILocalUserRepository repo, PasswordHasher passwordHasher, JwtService jwtService)
    {
        _repo = repo;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public string SignUp(string username, string? email, string password)
    {
        if (_repo.GetByUsername(username) != null)
            throw new ArgumentException("Username already exists");
        if (!string.IsNullOrEmpty(email) && _repo.GetByEmail(email) != null)
            throw new ArgumentException("Email already exists");

        var userId = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16)).Replace("+", "-").Replace("/", "_").TrimEnd('=');
        var user = new Entities.LocalUser
        {
            Id = userId,
            Username = username,
            Email = email,
            PasswordHash = _passwordHasher.HashPassword(password)
        };
        _repo.Create(user);

        return _jwtService.CreateAccessToken(userId, "local", email);
    }

    public string SignIn(string username, string password)
    {
        var user = _repo.GetByUsername(username);
        if (user == null)
            throw new ArgumentException("User not found for the provided username.");
        if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
            throw new ArgumentException("Invalid password.");

        return _jwtService.CreateAccessToken(user.Id, "local", user.Email);
    }
}

