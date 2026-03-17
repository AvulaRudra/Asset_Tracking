namespace dotnet_backend.DTOs;

public class TokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "bearer";
}

