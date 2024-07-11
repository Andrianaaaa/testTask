using Infrastructure.Notification;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers;

[ApiController]
[Route("/notification")]
public class NotificationController: ControllerBase
{
    [HttpPost("notify")]
    public Task<IActionResult> Notify([FromBody] NotificationData notificationData)
    {
        var message = GenerateNotificationMessage(notificationData);
        
        Console.WriteLine(message);
        
        return Task.FromResult<IActionResult>(Ok(new NotificationResponse(message)));
    }

    private record NotificationResponse(string Notification);

    // This method should be moved to a service class
    private static string GenerateNotificationMessage(NotificationData notificationData)
    {
        return notificationData.ActionType switch
        {
            (int)ActionType.UserAuthorized => $"{notificationData.FirstName} {notificationData.LastName} ({notificationData.Email}) has been authorized.",
            (int)ActionType.UserProfileLoaded => $"{notificationData.FirstName} {notificationData.LastName} ({notificationData.Email})'s profile has been loaded.",
            _ => throw new ArgumentOutOfRangeException(nameof(notificationData.ActionType))
        };
    }
    
}