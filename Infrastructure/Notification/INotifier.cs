namespace Infrastructure.Notification;

public interface INotifier
{
    Task<bool> Notify(NotificationData notificationData);
}