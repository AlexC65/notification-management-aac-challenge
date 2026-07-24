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
/// <remarks>
/// Every action in this controller requires the caller to be authenticated
/// (see <see cref="AuthorizeAttribute"/>). Requests without a valid bearer
/// token will receive a 401 Unauthorized response.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public sealed class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    // ── GET /api/notifications?page=1&pageSize=20 ───────────────────────────

    /// <summary>
    /// Returns a paginated list of notifications belonging to the authenticated user.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/notifications?page=1&amp;pageSize=20
    ///
    /// </remarks>
    /// <param name="page">Page number to retrieve (1-based). Defaults to 1.</param>
    /// <param name="pageSize">Number of items per page. Defaults to 20.</param>
    /// <param name="ct">Token to cancel the operation.</param>
    /// <response code="200">Returns the paginated list of notifications for the current user.</response>
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
    /// </summary>
    /// <remarks>
    /// The notification is dispatched synchronously as part of this call. The response
    /// includes a friendly <c>notificationId</c> that can be used to look up the
    /// notification later (e.g. via <c>GET /api/notifications</c>).
    ///
    /// Sample request:
    ///
    ///     POST /api/notifications
    ///     {
    ///        "Title": "Welcome!",
    ///        "Content": "Thanks for signing up.",
    ///        "Channel": "Email",
    ///        "Recipient": "user@example.com"
    ///     }
    ///
    /// </remarks>
    /// <param name="request">The notification payload, including channel, recipient and content.</param>
    /// <param name="ct">Token to cancel the operation.</param>
    /// <response code="201">The notification was created and dispatch was attempted.</response>
    /// <response code="400">The request payload is invalid (missing/invalid fields).</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request, CancellationToken ct)
    {
        var userId = GetUserId();
        var response = await _notificationService.CreateAsync(request, userId, ct);
        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Ok",
            notificationId = response.NotificationId
        });
    }
    // ── PUT /api/notifications/{id} ───────────────────────────────────────────

    /// <summary>
    /// Updates the title, content, and channel of an existing notification.
    /// The notification must belong to the authenticated user.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/notifications/42
    ///     {
    ///        
    ///        "Title": "Updated subject",
    ///        "body": "Updated content."
    ///        "Channel": "Email",
    ///        "Recipient": "user@example.com"
    ///     }
    /// 
    /// </remarks>
    /// <param name="notificationId">The identifier of the notification to update for this user.</param>
    /// <param name="request">The new values for title, content, and channel.</param>
    /// <param name="ct">Token to cancel the operation.</param>
    /// <response code="200">Returns the updated notification.</response>
    /// <response code="403">The notification belongs to a different user.</response>
    /// <response code="404">The notification does not exist.</response>
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
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/notifications/42
    ///
    /// </remarks>
    /// <param name="notificationId">The identifier of the notification to delete for this user.</param>
    /// <param name="ct">Token to cancel the operation.</param>
    /// <response code="204">The notification was deleted successfully.</response>
    /// <response code="403">The notification belongs to a different user.</response>
    /// <response code="404">The notification does not exist.</response> 

    [HttpDelete("{notificationId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int notificationId, CancellationToken ct)
    {
        var userId = GetUserId();
        await _notificationService.DeleteAsync(userId, notificationId, ct);
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
