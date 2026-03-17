namespace dotnet_backend.DTOs;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string? Email { get; set; }
}

