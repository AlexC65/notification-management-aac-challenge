using NotificationManagement.Domain.Enums;
namespace NotificationManagement.Application.DTOs.Notifications;
public sealed record UpdateNotificationRequest(
    string Title,
    string Content,
    ChannelType Channel,
    string Recipient 
);