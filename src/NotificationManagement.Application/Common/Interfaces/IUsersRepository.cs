using NotificationManagement.Domain.Entities;

namespace NotificationManagement.Application.Common.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User> GetUserByEmailAsync(string email);
}

