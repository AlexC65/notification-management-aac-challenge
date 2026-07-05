using NotificationManagement.Domain.Entities;
using NotificationManagement.Domain.Enums;
using NotificationManagement.Application.DTOs.Notifications;


namespace NotificationManagement.Application.Interfaces;

public interface INotificationService
{
        Task<NotificationResponse> CreateAsync(CreateNotificationRequest request,
                                               Guid userId,
                                               CancellationToken ct = default);
        Task<NotificationResponse> UpdateAsync(Guid userId, 
                                                int notificationId, 
                                                UpdateNotificationRequest request,  
                                                CancellationToken ct = default);
 
        Task DeleteAsync(Guid userId,
                         int notificationId,
                         CancellationToken ct = default);
 
        Task<IEnumerable<NotificationResponse>> GetMyNotificationsAsync(Guid userId,
                                                                        int page = 1,
                                                                        int pageSize = 20,
                                                                        CancellationToken ct = default);
}