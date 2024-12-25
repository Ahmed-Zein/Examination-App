using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class LoginRequestDto
{
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public class RegisterRequestDto : LoginRequestDto
{
    [Required]
    [StringLength(256, MinimumLength = 3)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(256, MinimumLength = 3)]
    public string LastName { get; set; } = string.Empty;
}

public class AuthenticationResponseDto
{
    public string Token { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public static AuthenticationResponseDto Success(string token, string message)
    {
        return
            new AuthenticationResponseDto
                { Message = message, Token = token };
    }

    public static AuthenticationResponseDto Fail(string message)
    {
        return
            new AuthenticationResponseDto
                { Message = message };
    }
}