using System.Threading.Tasks;
using NotificationManagement.Domain.Entities;

namespace NotificationManagement.Application.Interfaces
{
    public interface ISendNotification
    {
        //Validate
        Task<bool> ValidateAsync(string recipient);
        //BuildMessage
        Task<string> BuildMessageAsync(string message);
        //RecordDelivery
        Task RecordDeliveryAsync(Notification notification);

        //Task SendNotificationAsync(Notification notification);
    }


}    