using dotnet_backend.Repositories;
using dotnet_backend.Core;

namespace dotnet_backend.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;

    public AuthService(IUserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public string HandleSsoLogin(string provider, string subject, string? email)
    {
        _userRepository.Upsert(provider, subject, email);
        return _jwtService.CreateAccessToken(subject, provider, email);
    }
}

