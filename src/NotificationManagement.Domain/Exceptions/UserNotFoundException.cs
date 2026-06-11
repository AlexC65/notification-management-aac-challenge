using System;

namespace NotificationManagement.Domain.Exceptions
{
    // Represents errors that occur related to User domain operations.
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(string email)
            : base($"User with email {email} was not found.")
        {
        }
    }
}
