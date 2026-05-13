namespace NotificationManagement.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string UserMail { get; set; }
    public string UserName { get; set; }
    public string UserPassword { get; set; }
    public DateTime RegistrationDate { get; set; }

    public User(string userMail, string userName, string userPassword)
    {
        if (string.IsNullOrEmpty(userMail))
            throw new ArgumentException("User mail cannot be null or empty.", nameof(userMail));
        if (string.IsNullOrEmpty(userName))
            throw new ArgumentException("User name cannot be null or empty.", nameof(userName));
        if (string.IsNullOrEmpty(userPassword))
            throw new ArgumentException("User password cannot be null or empty.", nameof(userPassword));

        Id = Guid.NewGuid();
        UserMail = userMail;
        UserName = userName;
        UserPassword = userPassword;
        RegistrationDate = DateTime.UtcNow;
    }
}
