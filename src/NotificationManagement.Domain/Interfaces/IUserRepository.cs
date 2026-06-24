using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationManagement.Domain.Entities;

namespace NotificationManagement.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string emai, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
    }
}
