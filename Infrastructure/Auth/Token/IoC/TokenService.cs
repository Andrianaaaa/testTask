using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.AuthToken;

public class TokenService : ITokenService
{
    public string GetEmailFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null)
            throw new ArgumentException("Invalid token");

        var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value;

        if (emailClaim == null)
            throw new ArgumentException("Invalid email claim in token.");

        return emailClaim;
    }
}