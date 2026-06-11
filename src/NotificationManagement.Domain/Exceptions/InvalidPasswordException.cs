namespace NotificationManagement.Domain.Exceptions
{
    public sealed class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException()
            : base("The provided password is invalid.")
        {
        }
    }
}