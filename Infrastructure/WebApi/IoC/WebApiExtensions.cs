using Microsoft.AspNetCore.Builder;

namespace Infrastructure;

// Extension method to setup everything for web api
public static class WebApiExtensions
{
    public static IApplicationBuilder UseWebApi(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server v1"));
            
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}
