using API.DataModel;
using API.Services;
using Infrastructure;
using Infrastructure.AuthToken;
using Infrastructure.AuthToken.IoC;
using Infrastructure.Notification.IoC;
using Infrastructure.Swagger.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                                                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddScoped<IHasherService, Sha256HasherServiceService>();
builder.Services.AddTransient<ITokenProvider, JwtTokenProvider>();

builder.Services.AddAuthToken(builder.Configuration);
builder.Services.AddNotification();

var app = builder.Build();

app.UseWebApi();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();