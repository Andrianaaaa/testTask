using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.AuthToken;

public class JwtTokenProvider : ITokenProvider
{
    private readonly IConfiguration _configuration;

    public JwtTokenProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetToken(UserTokenData tokenData)
    {
        return GenerateJwtToken(tokenData);
    }

    private string GenerateJwtToken(UserTokenData tokenData)
    {
        EnsureConfigValidation();

        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
                         {
                             new Claim(JwtRegisteredClaimNames.Email, tokenData.Email),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                         };

            var token = new JwtSecurityToken(
                                             issuer: _configuration["Jwt:Issuer"],
                                             audience: _configuration["Jwt:Audience"],
                                             claims: claims,
                                             expires: DateTime.Now.AddMinutes(30),
                                             signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"ArgumentException: {ex.Message}");
            throw new InvalidOperationException("Invalid arguments provided for token creation.", ex);
        }
        catch (SecurityTokenException ex)
        {
            Console.WriteLine($"SecurityTokenException: {ex.Message}");
            throw new InvalidOperationException("Security token error.", ex);
        }
        catch (ConfigurationErrorsException ex)
        {
            Console.WriteLine($"ConfigurationErrorsException: {ex.Message}");
            throw new InvalidOperationException("Configuration error.", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            throw new InvalidOperationException("An error occurred while generating the token.", ex);
        }
    }

    private void EnsureConfigValidation()
    {
        if (_configuration["Jwt:Key"] is null)
        {
            throw new ConfigurationErrorsException("Jwt:Key is not configured.");
        }

        if (_configuration["Jwt:Issuer"] is null)
        {
            throw new ConfigurationErrorsException("Jwt:Issuer is not configured.");
        }

        if (_configuration["Jwt:Audience"] is null)
        {
            throw new ConfigurationErrorsException("Jwt:Audience is not configured.");
        }
    }
}