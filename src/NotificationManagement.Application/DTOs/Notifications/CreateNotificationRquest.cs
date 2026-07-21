using NotificationManagement.Domain.Enums;
namespace NotificationManagement.Application.DTOs.Notifications;

/// <summary>
/// Payload required to create and dispatch a new notification.
/// </summary>
/// <param name="Title">Short title/subject of the notification.</param>
/// <param name="Content">Body content of the notification.</param>
/// <param name="Channel">Channel used to send the notification (e.g. Email, Sms, Push).</param>
/// <param name="Recipient">Destination address (email, phone number, device token, etc.), depending on the channel.</param>

public sealed record CreateNotificationRequest(
    string Title,
    string Content,
    ChannelType Channel,
    string Recipient 
);