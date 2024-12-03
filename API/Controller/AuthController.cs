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
        // flow:
        // we need a Signup DTO, Response
        // the infra service takes that dto
        // what happens in the infra stays in the infra xd
        // ... create a user model
        // ... add role to user
        // ... return the user
        // if what is returned is ok return status
        var res = await authService.Register(registerRequestDto);
        return res.IsSuccess ? Ok(res.Value) : BadRequest(res.Errors);
    }

    [HttpPost("login")]
    public Task<ActionResult<AuthenticationResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        return Task.FromResult<ActionResult<AuthenticationResponseDto>>(
            Ok(new AuthenticationResponseDto { Token = "", message = "hola", success = true }));
    }
}