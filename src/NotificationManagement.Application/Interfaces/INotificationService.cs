using NotificationManagement.Domain.Entities;
using NotificationManagement.Domain.Enums;
using NotificationManagement.Application.DTOs.Notifications;


namespace NotificationManagement.Application.Interfaces;

public interface INotificationService
{
        Task<NotificationResponse> CreateAsync(
                                    Guid userId,
                                    string title,
                                    string content,
                                    ChannelType channel,
                                    string recipient,
                                    CancellationToken ct = default);

        Task<NotificationResponse> UpdateAsync(
                                    Guid Id,
                                    UpdateNotificationRequest request,
                                    Guid userId,
                                    CancellationToken ct = default);
 
        Task DeleteAsync(
                Guid Id,
                Guid userId,
                CancellationToken ct = default);
 
        Task<IEnumerable<NotificationResponse>> GetMyNotificationsAsync(
                Guid userId,
                int page = 1,
                int pageSize = 20,
                CancellationToken ct = default);
}