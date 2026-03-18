using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_backend.Entities;

public class User
{
    [Key, Column(Order = 0)]
    [MaxLength(64)]
    public string Id { get; set; } = string.Empty;

    [Key, Column(Order = 1)]
    [MaxLength(32)]
    public string Provider { get; set; } = string.Empty;

    [MaxLength(320)]
    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(20)]
    public string Role { get; set; } = "Viewer";
}



