using Application.DTOs;
using Core.Entities;
using FluentResults;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<Result<AppUser>> Register(RegisterRequestDto registerRequestDto);
}