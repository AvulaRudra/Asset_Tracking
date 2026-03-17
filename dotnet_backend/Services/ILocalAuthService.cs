namespace dotnet_backend.Services;

public interface ILocalAuthService
{
    string SignUp(string username, string? email, string password);
    string SignIn(string username, string password);
}

