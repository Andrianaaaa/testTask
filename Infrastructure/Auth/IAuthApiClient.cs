using System.Text.Json.Serialization;
using Refit;

namespace Infrastructure.Auth;

public interface IAuthApiClient
{
    [Post("/auth/login")]
    Task<ApiResponse<TokenResponse>> AuthenticateUserAsync([Body] LoginData loginData);

    [Get("/profile")]
    Task<ApiResponse<ProfileResponse>> GetProfileDataAsync([Header("Authorization")] string authorization);
}
public class LoginData
{
    public string Username { get; set; }
    public string Password { get; set; }
}

[Serializable]
public record TokenResponse(
    [property: JsonPropertyName("token")] string Token
);

[Serializable]
public record ProfileResponse(
    [property: JsonPropertyName("firstName")] string FirstName,
    [property: JsonPropertyName("lastName")] string LastName,
    [property: JsonPropertyName("email")] string Email
);