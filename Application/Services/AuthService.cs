using Application.DTOs;
using Application.Interfaces;
using Core.Constants;
using Core.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthService(UserManager<AppUser> userManager, ITokenService tokenService) : IAuthService
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
            return Result.Fail<AuthenticationResponseDto>(identityResult.Errors.Select(e => e.Description));

        List<string> roles = [AuthRolesConstants.Student];
        identityResult = await userManager.AddToRolesAsync(user, roles);

        if (!identityResult.Succeeded)
        {
            await userManager.DeleteAsync(user); // roll back
            return Result.Fail<AuthenticationResponseDto>(identityResult.Errors.Select(e => e.Description));
        }

        var token = tokenService.GenerateToken(user, roles);
        return Result.Ok(new AuthenticationResponseDto()
            { Success = true, Message = "successfully registered", Token = token });
    }

    public async Task<Result<AuthenticationResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var credentialsResult = await CheckCredentials(loginRequestDto.Email, loginRequestDto.Password);

        if (credentialsResult.IsFailed)
            return Result.Fail<AuthenticationResponseDto>(credentialsResult.Errors)
                .WithValue(new AuthenticationResponseDto()
                    { Success = false, Errors = credentialsResult.Errors.Select(e => e.Message).ToList() });

        var user = credentialsResult.Value;
        var roles = await userManager.GetRolesAsync(user);
        var token = tokenService.GenerateToken(user, roles);

        return Result.Ok(
            new AuthenticationResponseDto() { Token = token, Success = true, Message = "Login successful" });
    }

    public async Task<Result<AppUser>> CheckCredentials(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null || !await userManager.CheckPasswordAsync(user, password))
            return Result.Fail<AppUser>("Invalid credentials");

        return Result.Ok(user);
    }
}