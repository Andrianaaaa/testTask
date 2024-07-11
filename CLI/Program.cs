using System.IdentityModel.Tokens.Jwt;
using Infrastructure.Auth;
using Refit;

Console.WriteLine("Enter your username:");
var username = Console.ReadLine();

// additional validation could be added here
if (username is null)
{
    throw new ArgumentException("username cannot be null.");
}

Console.WriteLine("Enter your password:");
var password = Console.ReadLine();

// additional validation could be added here
if (password is null)
{
    throw new ArgumentException("password cannot be null.");
}

var apiService = RestService.For<IAuthApiClient>("http://localhost:8080");

var loginData = new LoginData { 
                                  Username = username, 
                                  Password = password 
                              };

var tokenResponse = await apiService.AuthenticateUserAsync(loginData);

if (tokenResponse is { IsSuccessStatusCode: true, Content.Token: not null })
{
    var token = tokenResponse.Content.Token;
    var profileResponse = await apiService.GetProfileDataAsync($"Bearer {token}");

    if (profileResponse.IsSuccessStatusCode)
    {
        Console.WriteLine("Profile Data:");
        Console.WriteLine(WriteProfileData(profileResponse.Content));
    }
    else
    {
        Console.WriteLine("Failed to retrieve profile data.");
        Console.WriteLine($"Status Code: {profileResponse.StatusCode}");
        Console.WriteLine($"Reason: {profileResponse.ReasonPhrase}");
    }
}
else
{
    Console.WriteLine("Authentication failed.");
    Console.WriteLine($"Status Code: {tokenResponse.StatusCode}");
    Console.WriteLine($"Reason: {tokenResponse.ReasonPhrase}");
}

return;

static string WriteProfileData(ProfileResponse profileResponse)
{
    return $"First Name: {profileResponse.FirstName}\n" +
           $"Last Name: {profileResponse.LastName}\n" +
           $"Email: {profileResponse.Email}";
}


