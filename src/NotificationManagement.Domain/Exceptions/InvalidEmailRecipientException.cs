namespace NotificationManagement.Domain.Exceptions;

// Email channel — invalid recipient format
public class InvalidEmailRecipientException : DomainException
{
    public InvalidEmailRecipientException(string email)
        : base($"The email address '{email}' has an invalid format.")
    {
    }
}