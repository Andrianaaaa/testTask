using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Infrastructure.Swagger.IoC;

public static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
            services.AddSwaggerGen(options =>
                                   {
                                       options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                                                                               {
                                                                                   Name = "Authorization",
                                                                                   Type = SecuritySchemeType.Http,
                                                                                   Scheme = "bearer",
                                                                                   BearerFormat = "JWT",
                                                                                   In = ParameterLocation.Header,
                                                                                   Description =
                                                                                       "JWT Authorization header using the Bearer scheme."
                                                                               });

                                       options.AddSecurityRequirement(new OpenApiSecurityRequirement
                                                                      {
                                                                          {
                                                                              new OpenApiSecurityScheme
                                                                              {
                                                                                  Reference = new OpenApiReference
                                                                                              {
                                                                                                  Type = ReferenceType
                                                                                                      .SecurityScheme,
                                                                                                  Id = "Bearer"
                                                                                              }
                                                                              },
                                                                              Array.Empty<string>()
                                                                          }
                                                                      });
                                   });

            return services;
        }
}