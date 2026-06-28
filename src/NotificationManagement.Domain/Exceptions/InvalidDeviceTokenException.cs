namespace NotificationManagement.Domain.Exceptions;

// Push channel — device token is missing or malformed
public class InvalidDeviceTokenException : DomainException
{
    public InvalidDeviceTokenException(string token)
        : base($"Device token '{token}' is invalid or expired.")
    {
    }
}