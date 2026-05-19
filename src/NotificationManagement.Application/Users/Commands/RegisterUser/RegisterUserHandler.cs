using NotificationManagement.Application.Common.Interfaces;

namespace NotificationManagement.Application.Users.Commands.RegisterUser;
public class RegisterUserHandler
{
    private readonly IUserRepository userRepository;
    private readonly IPasswordHasher passwordHasherters;

    public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasherters)
{
        this.userRepository = userRepository;
        this.passwordHasherters = passwordHasherters;
    }
}