using System.ComponentModel.DataAnnotations;

namespace dotnet_backend.DTOs;

public class SignUpRequestDto
{
    [MinLength(3), MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    public string? Email { get; set; }

    [MinLength(8), MaxLength(128)]
    public string Password { get; set; } = string.Empty;
}

