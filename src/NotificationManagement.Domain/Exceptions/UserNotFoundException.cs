using System;

namespace NotificationManagement.Domain.Exceptions
{
    // Represents errors that occur related to User domain operations.
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(string message)
            : base(message)
        {
        }

        public UserNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
