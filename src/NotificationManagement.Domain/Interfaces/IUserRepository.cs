using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationManagement.Domain.Entities;

namespace NotificationManagement.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
