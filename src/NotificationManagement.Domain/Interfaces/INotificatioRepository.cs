using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationManagement.Domain.Entities;

namespace NotificationManagement.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification, CancellationToken ct = default);

        Task UpdateAsync(Notification notification, CancellationToken ct = default);

        Task DeleteAsync(Notification notification, CancellationToken ct = default);

        Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<IEnumerable<Notification>> GetByUserIdAsync(
                                            Guid userId,
                                            int page = 1,
                                            int pageSize = 20,
                                            CancellationToken ct = default);
    }
}
