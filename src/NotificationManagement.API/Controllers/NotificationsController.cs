using Microsoft.AspNetCore.Mvc;
using NotificationManagement.Application.Interfaces;
using NotificationManagement.Application.DTOs.Auth;
using NotificationManagement.Application.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace NotificationManagement.API.Controllers;
/// <summary>
/// CRUD + dispatch for notifications. All endpoints require a valid JWT.
/// </summary>

[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>Returns a paginated list of notifications belonging to the authenticated user.</summary>
    [HttpGet]
    public async Task<IActionResult> GetMine(
                                        [FromQuery] int page = 1,
                                        [FromQuery] int pageSize = 20,
                                        CancellationToken ct = default)
    {
        var userId = GetUserId();
        var notifications = await _notificationService
            .GetMyNotificationsAsync(userId, page, pageSize, ct);
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
