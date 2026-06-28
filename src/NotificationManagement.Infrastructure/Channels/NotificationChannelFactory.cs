using NotificationManagement.Application.Interfaces;
using NotificationManagement.Domain.Enums;
using NotificationManagement.Domain.Exceptions;
using NotificationManagement.Domain.Interfaces;

namespace NotificationManagement.Infrastructure.Channels;

public class NotificationChannelFactory : INotificationChannelFactory
{
    private readonly IReadOnlyDictionary<ChannelType, INotificationChannel> _channels;
    
    public NotificationChannelFactory(IEnumerable<INotificationChannel> channels)
    {
         _channels = channels.ToDictionary(c => c.Channel);
    }

    public INotificationChannel Resolve(ChannelType channel)
    {
        if (!_channels.TryGetValue(channel, out var notificationChannel))
            throw new NotificationChannelException(
                channel.ToString(),
                "No handler registered for this channel.");

        return notificationChannel;
    }
}