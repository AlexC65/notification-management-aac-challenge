using NotificationManagement.Domain.Entities;
using NotificationManagement.Domain.Enums;


namespace NotificationManagement.Domain.Interfaces;

/// <summary>
/// Strategy interface for notification channels.
/// Adding a new channel only requires implementing this interface —
/// no existing code needs to change (Open/Closed Principle).
/// </summary>
public interface INotificationChannel
{
    ChannelType Channel { get; }
    Task SendAsync(Notification notification, CancellationToken cancellationToken = default);
}
