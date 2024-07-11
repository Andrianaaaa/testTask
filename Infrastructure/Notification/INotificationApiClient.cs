using System.Text.Json.Serialization;
using Refit;

namespace Infrastructure.Notification;

public interface INotificationApiClient
{
    [Post("/notification/notify")]
    Task<ApiResponse<NotificationResponse>> Notify([Body]NotificationData notificationData);
}

[Serializable]
public record NotificationData(
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("firstName")] string FirstName,
    [property: JsonPropertyName("lastName")] string LastName,
    [property: JsonPropertyName("actionType")] int ActionType
);

[Serializable]
public record NotificationResponse(
    [property: JsonPropertyName("notification")] string Notification
);

public enum ActionType
{
    Undefined = 0,
    UserAuthorized,
    UserProfileLoaded,
}