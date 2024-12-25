using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static System.Security.Claims.ClaimTypes;

namespace Application.Services;

public sealed class TokenService(IConfiguration config) : ITokenService
{
    private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(
        config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT:SECRET can't be NULL")));

    public string GenerateToken(AppUser user, IList<string> roles)
    {
        var claims = CreateClaims(user, roles);

        var signInCredential = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT:ISSUER can't be NULL"),
            Audience = config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT:AUDIENCE can't be NULL"),
            SigningCredentials = signInCredential
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public List<Claim> CreateClaims(AppUser user, IList<string> roles)
    {
        return
        [
            new Claim(Role, string.Join(",", roles)),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];
    }
}