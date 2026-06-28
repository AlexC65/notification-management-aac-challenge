namespace NotificationManagement.Domain.Exceptions;

// When title or content is null/empty
public class EmptyNotificationContentException : DomainException
{
    public EmptyNotificationContentException(string fieldName)
        : base($"The field '{fieldName}' cannot be empty.")
    {
    }
}