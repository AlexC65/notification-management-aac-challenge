
using NotificationManagement.Domain.Entities;
using NotificationManagement.Domain.Interfaces;
using NotificationManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace NotificationManagement.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _db;

    public NotificationRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Notification notification, CancellationToken ct = default)
    {
        using var transaction = await _db.Database.BeginTransactionAsync(ct);

        try
        {
            var nextSequence = await _db.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == notification.UserId)
                .CountAsync(ct) + 1;

            notification.SetSequenceNumber(nextSequence);

            await _db.Notifications.AddAsync(notification, ct);
            await _db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch 
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task DeleteAsync(Notification notification, CancellationToken ct = default)
    {
        _db.Notifications.Remove(notification);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        object[] keyValues = new object[] { id };
            
        return await _db.Notifications
                        .FindAsync(keyValues, ct);
    }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId,
                                                                    int page = 1,
                                                                    int pageSize = 20,
                                                                    CancellationToken ct = default)
    {
        return await  _db.Notifications
                        .AsNoTracking()
                        .Where(n => n.UserId == userId)
                        .OrderByDescending(n => n.CreatedDate)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync(ct);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken ct = default)
    {
        _db.Notifications.Update(notification);
        await _db.SaveChangesAsync(ct);
    }
}