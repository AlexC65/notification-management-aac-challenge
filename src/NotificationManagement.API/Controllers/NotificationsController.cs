using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationManagement.Application.DTOs.Auth;
using NotificationManagement.Application.DTOs.Notifications;
using NotificationManagement.Application.Interfaces;
using NotificationManagement.Application.Services;

namespace NotificationManagement.API.Controllers;
/// <summary>
/// CRUD + dispatch for notifications. All endpoints require a valid JWT.
/// </summary>
[ApiController]
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

    // ── GET /api/notifications?page=1&pageSize=20 ───────────────────────────

    /// <summary>Returns a paginated list of notifications belonging to the authenticated user.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NotificationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMine(
                                        [FromQuery] int page = 1,
                                        [FromQuery] int pageSize = 20,
                                        CancellationToken ct = default)
    {
        var userId = GetUserId();
        var notifications = await _notificationService.GetMyNotificationsAsync(userId, page, pageSize, ct);
        return Ok(notifications);
    }

    // ── POST /api/notifications ───────────────────────────────────────────────

    /// <summary>
    /// Creates a notification and immediately dispatches it through the specified channel.
    /// The response includes the resulting status (Sent / Failed) and a friendly NotificationId.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
    [FromBody] CreateNotificationRequest request,
    CancellationToken ct)
    {
        var userId = GetUserId();
        var response = await _notificationService.CreateAsync(request, userId, ct);
        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Ok",
            sequenceNumber = response.NotificationId
        });
    }

    // ── PUT /api/notifications/{id} ───────────────────────────────────────────

    /// <summary>
    /// Updates the title, content, and channel of an existing notification.
    /// The notification must belong to the authenticated user.
    /// </summary>
    /// <param name="notificationId">The identifier of the notification to update for this user.</param>
    /// <param name="request">The new values for title, content, and channel.</param>
    /// <param name="ct">Token to cancel the operation.</param>
    /// <returns>
    /// 200 OK — returns the updated notification.
    /// 404 Not Found — notification does not exist.
    /// 403 Forbidden — notification belongs to a different user.
    /// </returns>
    [HttpPut("{notificationId:int}")]
    [ProducesResponseType(typeof(NotificationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(int notificationId,
                                            [FromBody] UpdateNotificationRequest request,
                                            CancellationToken ct)
    {
        var userId = GetUserId();
        var response = await _notificationService.UpdateAsync(userId, notificationId, request, ct);
        return Ok(response);
    }

    // ── DELETE /api/notifications/{id} ────────────────────────────────────────

    /// <summary>
    /// Deletes an existing notification belonging to the authenticated user.
    /// The notification is identified by its sequence number.
    /// </summary>
    /// <param name="notificationId">The identifier of the notification to delete for this user.</param>
    /// <param name="ct">Token to cancel the operation.</param>
    /// <returns>
    /// 204 No Content — notification deleted successfully.
    /// 404 Not Found — notification does not exist.
    /// 403 Forbidden — notification belongs to a different user.
    /// </returns> 

    [HttpDelete("{notificationId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int notificationId, CancellationToken ct)
    {
        var userId = GetUserId();
        await _notificationService.DeleteAsync(userId, notificationId, ct); ;
        return NoContent();
    }


    // ── Helper ──────────────────────────────────────────────────────────────── 
    private Guid GetUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User ID claim is missing from the token.");

        return Guid.Parse(claim);
    }
}
