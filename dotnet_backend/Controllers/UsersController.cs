using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet_backend.Data;
using dotnet_backend.DTOs;
using dotnet_backend.Entities;
using dotnet_backend.Repositories;
using dotnet_backend.Services;
using System.Security.Claims;
using System.Security.Claims;
using static dotnet_backend.Entities.UserRoles;

namespace dotnet_backend.Controllers;

[ApiController]
[Route("users")]
[Tags("users")]
[Authorize] 
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly ILocalUserRepository _localRepo;

    public UsersController(IUserRepository userRepo, ILocalUserRepository localRepo)
    {
        _userRepo = userRepo;
        _localRepo = localRepo;
    }

    [HttpGet("me")]
    public IActionResult GetMe()
    {
        var userId = User.FindFirst("sub")?.Value;
        var provider = User.FindFirst("provider")?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value ?? Viewer;
        return Ok(new UserDto { Id = userId ?? "", Provider = provider ?? "", Email = email, Role = role });
    }
    [HttpPatch("{identifier}/role")]
// [Authorize(Policy = "Admin")] // keep this if you already have admin policy
    public async Task<ActionResult<UserDto>> UpdateUserRole(string identifier,[FromBody] UpdateUserRoleDto dto)
    {
    // 1️⃣ Validate role
    if (!UpdateUserRoleDto.IsValidRole(dto.NewRole))
    {
        return BadRequest("Invalid role");
    }

    var callerId = User.FindFirst("sub")?.Value;
    var callerProvider = User.FindFirst("provider")?.Value ?? "local";

    UserDto updatedUser;

    /*
     ✅ Identifier formats supported:
        - google/{id}
        - azure/{id}
        - local user by email or id (admin@gmail.com / 123)
    */

    // 2️⃣ SSO users: provider/id
    if (identifier.StartsWith("google/") || identifier.StartsWith("azure/"))
    {
        var parts = identifier.Split('/', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            return BadRequest("Invalid identifier format. Use provider/id");
        }

        var provider = parts[0];
        var externalId = parts[1];

        // Update role
        await _userRepo.UpdateRoleAsync(provider, externalId, dto.NewRole);

        var user = _userRepo.GetByProviderId(provider, externalId);
        if (user == null)
        {
            return NotFound();
        }

        updatedUser = new UserDto
        {
            Id = user.Id,
            Provider = user.Provider,
            Email = user.Email,
            Role = user.Role
        };
    }
    // 3️⃣ Local users: email OR id
    else
    {
        var localUser = await _localRepo.GetByIdOrEmailAsync(identifier);
        if (localUser == null)
        {
            return NotFound();
        }

        await _localRepo.UpdateRoleAsync(localUser.Id, dto.NewRole);

        updatedUser = new UserDto
        {
            Id = localUser.Id,
            Provider = "local",
            Email = localUser.Email,
            Role = localUser.Role
        };
    }

    // 4️⃣ Prevent admin from demoting self
    if (callerId == updatedUser.Id &&
        callerProvider == updatedUser.Provider &&
        updatedUser.Role != "Admin")
    {
        return Forbid("Cannot demote self from Admin");
    }

    return Ok(updatedUser);
   }
}

