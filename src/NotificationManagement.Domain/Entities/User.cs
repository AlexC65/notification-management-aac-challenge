namespace NotificationManagement.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Name { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime RegistrationDate { get; private set; }

    public User(string email, string name, string passwordHash)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException("User email cannot be null or empty.", nameof(email));
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("User name cannot be null or empty.", nameof(name));
        if (string.IsNullOrEmpty(passwordHash))
            throw new ArgumentException("User password cannot be null or empty.", nameof(passwordHash));

        Id = Guid.NewGuid();
        Email = email;
        Name = name;
        PasswordHash = passwordHash;
        RegistrationDate = DateTime.UtcNow;
    }
}
