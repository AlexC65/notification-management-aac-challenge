namespace NotificationManagement.Domain.Exceptions;
public class NotificationNotFoundException : DomainException
{
    public NotificationNotFoundException(int id)
        : base($"Notification with ID '{id}' was not found.")
    {
    }
}