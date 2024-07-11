using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Infrastructure.Notification.IoC;

public static class NotificationServiceCollectionExtensions
{
    public static IServiceCollection AddNotification(this IServiceCollection services)
    {
        services.AddRefitClient<INotificationApiClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://notification:8080"));
        
        services.AddTransient<INotifier, HttpNotifier>();
        
        return services;
    }
}