using API.DataModel;
using API.Services;
using Infrastructure.AuthToken;
using Infrastructure.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenProvider _tokenProvider;
    private readonly INotifier _notifier;
    private readonly IHasherService _hasherService;

    public AuthController(ApplicationDbContext context,
                          ITokenProvider tokenProvider,
                          INotifier notifier,
                          IHasherService hasherService)
    {
        _context = context;
        _tokenProvider = tokenProvider;
        _notifier = notifier;
        _hasherService = hasherService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginData loginData)
    {
        if (string.IsNullOrEmpty(loginData.Username) || string.IsNullOrEmpty(loginData.Password))
        {
            return BadRequest("Invalid login data.");
        }

        var user = await _context.Users
                            .SingleOrDefaultAsync(u => u.Email == loginData.Username);

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }
        
        if (!_hasherService.Verify(user.Password, loginData.Password))
        {
            return Unauthorized("Password is incorrect.");
        }

        var notificationResult = await _notifier.Notify(new NotificationData(user.Email,
                                              user.FirstName,
                                              user.LastName,
                                              (int)ActionType.UserAuthorized));

        Console.WriteLine(notificationResult 
                              ? "Notification sent successfully." 
                              : "Failed to send notification.");

        var token = _tokenProvider.GetToken(new UserTokenData(user.Email));
        
        return Ok(new TokenResponse(token));
    }
}

public record TokenResponse(string Token);

public class LoginData
{
    public string Username { get; set; }
    public string Password { get; set; }
}
