using NotificationManagement.Application.Interfaces;
using NotificationManagement.Domain.Entities;

namespace NotificationManagement.Application.Services
{
    public class SendNotificationServices
    {
        private  ISendNotification _sendNotification;

        public SendNotificationServices(ISendNotification sendNotification)
        {
            _sendNotification = sendNotification;
        }

        public void setSendNotification(ISendNotification sendNotification)
        {
            _sendNotification = sendNotification;
        }

        public async Task SendNotificationAsync(Notification notification)
        {
            if (await _sendNotification.ValidateAsync(notification.Recipient))
            {
                //var builtMessage = await _sendNotification.BuildMessageAsync(message);
                // Here you would typically create a Notification object and save it to the database
                // For example:
                // var notification = new Notification(userId, title, builtMessage, channel);
                // await _sendNotification.RecordDeliveryAsync(notification);
                Console.WriteLine($"Notification sent to {notification.Recipient} via {notification.Channel}");
            }
            else
            {
                throw new InvalidOperationException("Invalid message format.");
            }
        }
    }
}
