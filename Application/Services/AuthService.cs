using Application.DTOs;
using Application.Interfaces;
using Core.Constants;
using Core.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public sealed class AuthService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ITokenService tokenService) : IAuthService
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
            return Result.Fail(identityResult.Errors.Select(e => e.Description));

        List<string> roles = [AuthRolesConstants.Student];
        identityResult = await userManager.AddToRolesAsync(user, roles);

        if (!identityResult.Succeeded)
        {
            await userManager.DeleteAsync(user); // roll back
            return Result.Fail(identityResult.Errors.Select(e => e.Description));
        }

        var token = tokenService.GenerateToken(user, roles);
        return Result.Ok(AuthenticationResponseDto.Success(token, "Registered successfully"));
    }

    public async Task<Result<AuthenticationResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var signInResult =
            await signInManager.PasswordSignInAsync(loginRequestDto.Email, loginRequestDto.Password, false, false);

        if (!signInResult.Succeeded)
            return _handelInvalidLogin(signInResult);

        var user = (await signInManager.UserManager.FindByEmailAsync(loginRequestDto.Email))!;
        if (user.isLocked)
            return Result.Fail("Your account is locked");

        var roles = await userManager.GetRolesAsync(user);
        var token = tokenService.GenerateToken(user, roles);
        return Result.Ok(AuthenticationResponseDto.Success(token, "Login successful"));
    }

    public async Task<Result<AppUser>> CheckCredentials(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null || !await userManager.CheckPasswordAsync(user, password))
            return Result.Fail("Invalid credentials");

        return Result.Ok(user);
    }

    public async Task<Result<bool>> IsUserLockedOut(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is not null
            ? user.isLocked ? Result.Ok(user.isLocked).WithError("User locked out") : Result.Ok(user.isLocked)
            : Result.Fail("User does not exist");
    }

    private static Result<AuthenticationResponseDto> _handelInvalidLogin(SignInResult signInResult)
    {
        return signInResult switch
        {
            { IsLockedOut: true } => Result.Fail<AuthenticationResponseDto>(
                "Account is locked. Please try again later."),
            { RequiresTwoFactor: true } => Result.Fail<AuthenticationResponseDto>(
                "Two-factor authentication is required."),
            _ => Result.Fail<AuthenticationResponseDto>("Invalid credentials.")
        };
    }
}