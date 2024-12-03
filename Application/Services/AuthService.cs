using Application.DTOs;
using Application.Interfaces;
using Core.Constants;
using Core.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthService(UserManager<AppUser> userManager) : IAuthService
{
    /// ## flow:
    /// we need a Signup DTO, Response
    /// the infra service takes that dto
    /// what happens in the infra stays in the infra xd
    /// ... create a user model
    /// ... add role to user
    /// ... return the user
    /// if what is returned is ok return status
    public async Task<Result<AppUser>> Register(RegisterRequestDto registerRequestDto)
    {
        var user = new AppUser()
        {
            FirstName = registerRequestDto.FirstName,
            LastName = registerRequestDto.LastName,
            Email = registerRequestDto.Email,
            UserName = registerRequestDto.Email
        };

        var identityResult = await userManager.CreateAsync(user, registerRequestDto.Password);
        if (!identityResult.Succeeded)
            return Result.Fail<AppUser>(identityResult.Errors.Select(e => e.Description));

        identityResult = await userManager.AddToRoleAsync(user, AuthRolesConstants.Student);
        if (identityResult.Succeeded)
            return Result.Ok<AppUser>(user);

        await userManager.DeleteAsync(user);
        return Result.Fail<AppUser>(identityResult.Errors.Select(e => e.Description));
    }
}