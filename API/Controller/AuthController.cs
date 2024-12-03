using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult<AuthenticationResponseDto>> Signup([FromBody] RegisterRequestDto registerRequestDto)
    {
        var authResult = await authService.Register(registerRequestDto);

        return authResult switch
        {
            { IsSuccess: true } => Ok(authResult.Value),
            { IsSuccess: false } => BadRequest(authResult.Value),
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponseDto>> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var authResult = await authService.Login(loginRequestDto);

        return authResult switch
        {
            { IsSuccess: true } => Ok(authResult.Value),
            { IsSuccess: false } => BadRequest(authResult.Value),
        };
    }
}