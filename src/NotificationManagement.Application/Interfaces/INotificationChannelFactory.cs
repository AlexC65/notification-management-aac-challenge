using NotificationManagement.Domain.Enums;
using NotificationManagement.Domain.Interfaces;

namespace NotificationManagement.Application.Interfaces;
public interface INotificationChannelFactory
{
    INotificationChannel Resolve(ChannelType channel);
}