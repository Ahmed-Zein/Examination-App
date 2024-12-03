using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class LoginDto
{
    [Required, EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
}

public class RegisterRequestDto : LoginDto
{
    [Required, StringLength(maximumLength: 256, MinimumLength = 3)]
    public string FirstName { get; set; }

    [Required, StringLength(maximumLength: 256, MinimumLength = 3)]
    public string LastName { get; set; }
}

public class AuthenticationResponseDto
{
    public bool success { get; set; }
    public string Token { get; set; }
    public string message { get; set; }
}