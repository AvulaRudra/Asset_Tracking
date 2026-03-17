namespace dotnet_backend.Services;

public interface IAuthService
{
    string HandleSsoLogin(string provider, string subject, string? email);
}

