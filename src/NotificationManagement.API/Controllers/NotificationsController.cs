using Microsoft.AspNetCore.Mvc;
using NotificationManagement.Application.Interfaces;
using NotificationManagement.Application.DTOs.Auth;
using NotificationManagement.Application.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace NotificationManagement.API.Controllers;

[ApiController]
[Route("api/notifications")]
public sealed class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMine(
                                        [FromQuery] int page = 1,
                                        [FromQuery] int pageSize = 20,
                                        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        var notifications = await _notificationService
            .GetMyNotificationsAsync(userId, page, pageSize, cancellationToken);
        return Ok(notifications);
    }

    // ── Helper ──────────────────────────────────────────────────────────────── 
    private Guid GetUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User ID claim is missing from the token.");
 
        return Guid.Parse(claim);
    }
}
