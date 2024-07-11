using API.DataModel;
using Infrastructure.AuthToken;
using Infrastructure.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[Authorize]
[Route("/profile")]
public class ProfileController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly INotifier _notifier;
    private readonly ITokenService _tokenService;
    
    public ProfileController(ApplicationDbContext context,
                             INotifier notifier,
                             ITokenService tokenService)
    {
        _context = context;
        _notifier = notifier;
        _tokenService = tokenService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        try
        {
            var token = HttpContext.Request.Headers.Authorization!
                                   .ToString()["Bearer ".Length..].Trim();

            var email = _tokenService.GetEmailFromToken(token);

            var user = await _context.Users
                                     .SingleOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                return NotFound();
            }

            var notificationResult = await _notifier.Notify(new NotificationData(user.Email,
                                                                                 user.FirstName,
                                                                                 user.LastName,
                                                                                 (int)ActionType.UserProfileLoaded));
            Console.WriteLine(notificationResult
                                  ? "Notification sent successfully."
                                  : "Failed to send notification.");

            var response = new ProfileResponse(user.FirstName, user.LastName, user.Email);

            return Ok(response);
        }
        catch (SecurityTokenArgumentException ex)
        {
            Console.WriteLine("Failed to get email from token: " + ex.Message);
            return StatusCode(500);
        }
    }

    private record ProfileResponse(string FirstName, 
                                   string LastName,
                                   string Email);
}