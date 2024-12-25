using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult<JsonResponse<AuthenticationResponseDto>>> Signup(
        [FromBody] RegisterRequestDto registerRequestDto)
    {
        var authResult = await authService.Register(registerRequestDto);
        return authResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<AuthenticationResponseDto>.Ok(authResult.Value)),
            { IsSuccess: false } => BadRequest(JsonResponse<AuthenticationResponseDto>.Error(authResult.Errors))
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<JsonResponse<AuthenticationResponseDto>>> Login(
        [FromBody] LoginRequestDto loginRequestDto)
    {
        var authResult = await authService.Login(loginRequestDto);

        return authResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<AuthenticationResponseDto>.Ok(authResult.Value)),
            { IsSuccess: false } => Unauthorized(JsonResponse<AuthenticationResponseDto>.Error(authResult.Errors))
        };
    }
}