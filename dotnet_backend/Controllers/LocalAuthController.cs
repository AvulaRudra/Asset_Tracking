using Microsoft.AspNetCore.Mvc;
using dotnet_backend.DTOs;
using dotnet_backend.Services;

namespace dotnet_backend.Controllers;

[ApiController]
[Route("auth/local")]
[Tags("auth")]
public class LocalAuthController : ControllerBase
{
    private readonly ILocalAuthService _service;

    public LocalAuthController(ILocalAuthService service)
    {
        _service = service;
    }

    [HttpPost("signup")]
    public IActionResult Signup([FromBody] SignUpRequestDto dto)
    {
        try
        {
            var token = _service.SignUp(dto.Username, dto.Email, dto.Password);
            return Ok(new AuthResponseDto { AccessToken = token });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] SignInRequestDto dto)
    {
        try
        {
            var token = _service.SignIn(dto.Username, dto.Password);
            return Ok(new AuthResponseDto { AccessToken = token });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

