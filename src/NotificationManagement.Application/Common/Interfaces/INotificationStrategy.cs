using System.Threading.Tasks;
using NotificationManagement.Domain.Entities;

namespace NotificationManagement.Domain.Interfaces;

public interface INotificationStrategy
{
    Task SendNotificationAsync(Notification notification);
}

    