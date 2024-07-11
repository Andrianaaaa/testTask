namespace Infrastructure.Notification;

public class HttpNotifier: INotifier
{
    private readonly INotificationApiClient _notificationApiClient;

    public HttpNotifier(INotificationApiClient notificationApiClient)
    {
        _notificationApiClient = notificationApiClient;
    }

    public async Task<bool> Notify(NotificationData notificationData)
    {
        var response = await _notificationApiClient.Notify(notificationData);

        Console.WriteLine(response.IsSuccessStatusCode
                              ? $"Notification sent successfully: {response.Content.Notification}"
                              : $"Failed to send notification. Status Code: {response.StatusCode}, Error: {response.Error.Content}");

        return response.IsSuccessStatusCode;
    }
}