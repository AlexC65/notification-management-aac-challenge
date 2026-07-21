using NotificationManagement.Domain.Enums;

namespace NotificationManagement.Application.DTOs.Notifications;
/// <summary>
/// Represents a notification and the outcome of its dispatch attempt.
/// </summary>
/// <param name="Id">Unique identifier (GUID) of the notification.</param>
/// <param name="NotificationId">Friendly, human-readable sequence number for the notification, if assigned.</param>
/// <param name="UserId">Identifier of the user who owns this notification.</param>
/// <param name="Title">Short title/subject of the notification, if provided.</param>
/// <param name="Content">Body content of the notification.</param>
/// <param name="Channel">Channel used to send the notification (e.g. Email, Sms, Push).</param>
/// <param name="Recipient">Destination address (email, phone number, device token, etc.), if available.</param>
/// <param name="CreatedDate">Date and time (UTC) when the notification was created.</param>
/// <param name="Status">Current dispatch status of the notification (e.g. Sent, Failed).</param>
/// <param name="FailureReason">Reason the dispatch failed, if <see cref="Status"/> is Failed; otherwise null.</param>
public sealed record NotificationResponse(
    Guid Id,
    int? NotificationId,
    Guid UserId,
    string? Title,
    string Content,
    ChannelType Channel,
    string? Recipient,    
    DateTime CreatedDate,
    NotificationStatus Status,
    string? FailureReason
);