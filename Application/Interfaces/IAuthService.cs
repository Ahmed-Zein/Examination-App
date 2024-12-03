using Application.DTOs;
using Core.Entities;
using FluentResults;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthenticationResponseDto>> Register(RegisterRequestDto registerRequestDto);
    Task<Result<AuthenticationResponseDto>> Login(LoginRequestDto loginRequestDto);
    Task<Result<AppUser>> CheckCredentials(string email, string password);
}