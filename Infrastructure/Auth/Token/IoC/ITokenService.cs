namespace Infrastructure.AuthToken;

public interface ITokenService
{
    public string GetEmailFromToken(string token);
}