namespace NotificationManagement.Domain.Exceptions;

public class NotificationChannelException : DomainException
{
    public NotificationChannelException(string channel, string reason)
        : base($"Channel '{channel}' failed to send: {reason}") { }
}