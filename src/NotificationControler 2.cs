using NotificationManagement.Application.DTOs.Notifications;
using NotificationManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NotificationManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
        => _notificationService = notificationService;

    // ── GET /api/notifications?page=1&pageSize=20 ─────────────────────────────
    /// <summary>Returns a paginated list of notifications belonging to the authenticated user.</summary>

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NotificationResponse>), StatusCodes.Status200OK)]
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

    // ── GET /api/notifications/{id} ───────────────────────────────────────────

    /// <summary>Returns a single notification by id (must belong to the caller).</summary>

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(NotificationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var userId = GetUserId();
        var notification = await _notificationService.GetByIdAsync(id, userId, ct);
        return Ok(notification);
    }

    // ── POST /api/notifications ───────────────────────────────────────────────

        /// <summary>
    /// Creates a notification and immediately dispatches it through the specified channel.
    /// The response includes the resulting status (Sent / Failed) and a friendly SequenceNumber.
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
            sequenceNumber = response.SequenceNumber
        });
    }

    // ── PUT /api/notifications/{id} ───────────────────────────────────────────

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(NotificationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateNotificationRequest request,
        CancellationToken ct)
    {
        var userId = GetUserId();
        var response = await _notificationService.UpdateAsync(id, request, userId, ct);
        return Ok(response);
    }

    // ── DELETE /api/notifications/{id} ────────────────────────────────────────

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var userId = GetUserId();
        await _notificationService.DeleteAsync(id, userId, ct);
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