using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using dotnet_backend.DTOs;
using System.Security.Claims;
namespace dotnet_backend.Controllers;

[ApiController]
[Route("users")]
[Tags("users")]
[Authorize] 
public class UsersController : ControllerBase
{
    // Future impl, e.g. GET current user from token
    [HttpGet("me")]
    public IActionResult GetMe()
    {
        var userId = User.FindFirst("sub")?.Value;
        var provider = User.FindFirst("provider")?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        return Ok(new UserDto { Id = userId ?? "", Provider = provider ?? "", Email = email });
    }
}

