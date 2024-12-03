using Application.DTOs;
using Application.Interfaces;
using Core.Constants;
using Core.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public sealed class AuthService(UserManager<AppUser> userManager, ITokenService tokenService) : IAuthService
{
    public async Task<Result<AuthenticationResponseDto>> Register(RegisterRequestDto registerRequestDto)
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
            return Result.Fail<AuthenticationResponseDto>(identityResult.Errors.Select(e => e.Description))
                .WithValue(_failResponse(identityResult.Errors));

        List<string> roles = [AuthRolesConstants.Student];
        identityResult = await userManager.AddToRolesAsync(user, roles);

        if (!identityResult.Succeeded)
        {
            await userManager.DeleteAsync(user); // roll back
            return Result.Fail<AuthenticationResponseDto>(identityResult.Errors.Select(e => e.Description))
                .WithValue(_failResponse(identityResult.Errors));
        }

        var token = tokenService.GenerateToken(user, roles);
        return Result.Ok(_successResponse(token, "Registered successfully"));
    }

    public async Task<Result<AuthenticationResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var credentialsResult = await CheckCredentials(loginRequestDto.Email, loginRequestDto.Password);

        if (credentialsResult.IsFailed)
            return Result.Fail<AuthenticationResponseDto>(credentialsResult.Errors)
                .WithValue(_failResponse(credentialsResult.Errors));

        var user = credentialsResult.Value;
        var roles = await userManager.GetRolesAsync(user);
        var token = tokenService.GenerateToken(user, roles);

        return Result.Ok(_successResponse(token, "Login successful"));
    }

    public async Task<Result<AppUser>> CheckCredentials(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null || !await userManager.CheckPasswordAsync(user, password))
            return Result.Fail<AppUser>("Invalid credentials");

        return Result.Ok(user);
    }

    private static AuthenticationResponseDto _successResponse(string token, string message)
    {
        return
            new AuthenticationResponseDto()
                { Success = true, Message = message, Token = token };
    }

    private static AuthenticationResponseDto _failResponse(List<IError> errors)
    {
        return
            new AuthenticationResponseDto()
                { Success = false, Errors = errors.Select(e => e.Message).ToList() };
    }

    private static AuthenticationResponseDto _failResponse(IEnumerable<IdentityError> errors)
    {
        return
            new AuthenticationResponseDto()
                { Success = false, Errors = errors.Select(e => e.Description).ToList() };
    }
}