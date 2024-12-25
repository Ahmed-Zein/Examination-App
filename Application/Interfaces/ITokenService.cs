using System.Security.Claims;
using Core.Entities;

namespace Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(AppUser user, IList<string> roles);

    List<Claim> CreateClaims(AppUser user, IList<string> roles);
}