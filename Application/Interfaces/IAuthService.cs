using Application.DTOs;
using FluentResults;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthenticationResponseDto>> Register(RegisterRequestDto registerRequestDto);
    Task<Result<AuthenticationResponseDto>> Login(LoginRequestDto loginRequestDto);
    Task<Result<bool>> IsUserLockedOut(string userId);
}