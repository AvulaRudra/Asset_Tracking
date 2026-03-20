using System.ComponentModel.DataAnnotations;
using dotnet_backend.Entities;

namespace dotnet_backend.DTOs;

public class UpdateUserRoleDto
{
    [MaxLength(20)]
    public string NewRole { get; set; } = string.Empty;

    public static bool IsValidRole(string role)
    {
        return role == UserRoles.Viewer || role == UserRoles.Admin;
    }
}

