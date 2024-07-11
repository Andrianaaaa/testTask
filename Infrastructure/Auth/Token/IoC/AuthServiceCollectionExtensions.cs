using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.AuthToken.IoC;

//TODO: Check comments
public static class AuthServiceCollectionExtensions
{
    public static IServiceCollection AddAuthToken(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                              {
                                  options.TokenValidationParameters = new TokenValidationParameters
                                                                      {
                                                                          ValidateIssuer = true,
                                                                          ValidateAudience = true,
                                                                          ValidateLifetime = true,
                                                                          ValidateIssuerSigningKey = true,
                                                                          ValidIssuer = configuration["Jwt:Issuer"],
                                                                          // ValidIssuer = "https://localhost:5001",
                                                                          ValidAudience = configuration["Jwt:Audience"],
                                                                          // ValidAudience = "https://localhost:5001",
                                                                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                                                                          // IssuerSigningKey = new SymmetricSecurityKey("superSecretKey@345"u8.ToArray())
                                                                      };
                              });

        services.AddTransient<ITokenService, TokenService>();

        return services;
    }
}