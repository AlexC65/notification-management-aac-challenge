namespace NotificationManagement.Domain.Exceptions;

// SMS channel — content exceeds 160 characters
public class SmsContentTooLongException : DomainException
{
    public SmsContentTooLongException(int length)
        : base($"SMS content is too long ({length} chars). Maximum allowed is 160.")
    {
    }
}