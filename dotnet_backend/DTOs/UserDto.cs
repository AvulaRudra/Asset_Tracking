using System.ComponentModel.DataAnnotations;
using dotnet_backend.Entities;

namespace dotnet_backend.DTOs;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string? Email { get; set; }
    [MaxLength(20)]
    public string Role { get; set; } = UserRoles.Viewer;
}

