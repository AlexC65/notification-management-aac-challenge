using NotificationManagement.Domain.Enums;

namespace NotificationManagement.Application.DTOs.Notifications;
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