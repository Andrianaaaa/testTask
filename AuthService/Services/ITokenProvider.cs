namespace Infrastructure.AuthToken;

public interface ITokenProvider
{
    string GetToken(UserTokenData tokenData);
}

public record UserTokenData(string Email);