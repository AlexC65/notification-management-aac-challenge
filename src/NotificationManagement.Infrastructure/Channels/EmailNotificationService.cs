using NotificationManagement.Application.Interfaces;
using NotificationManagement.Domain.Entities;

namespace NotificationManagement.Infrastructure.Channels;

public class EmailNotificationService : ISendNotification
{
    public Task<string> BuildMessageAsync(string message)
    {
        throw new NotImplementedException();
    }

    public Task RecordDeliveryAsync(Notification notification)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateAsync(string recipient)
    {
        if (!recipient.Contains('@'))
            throw new ArgumentException("Recipient debe ser un email");
            
        return Task.FromResult(true);
    }
}
