using NotificationManagement.Domain.Interfaces;
using NotificationManagement.Application.Interfaces;
using NotificationManagement.Domain.Enums;
using NotificationManagement.Domain.Entities;
using NotificationManagement.Application.DTOs.Notifications;
using Microsoft.Extensions.Logging;
using NotificationManagement.Domain.Exceptions;


namespace NotificationManagement.Application.Services;

public sealed class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;
    private readonly INotificationChannelFactory _channelFactory;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(INotificationRepository repo, INotificationChannelFactory channelFactory,
                                ILogger<NotificationService> logger)
    {
        _repo = repo;
        _channelFactory = channelFactory;
        _logger = logger;
    }

    public async Task<NotificationResponse> CreateAsync(
                                    Guid userId,
                                    string title,
                                    string content,
                                    ChannelType channel,
                                    string recipient,
                                    CancellationToken ct = default)
    {
        var notification = Notification.Create(userId, title, content, channel, recipient);

        try
        {
            await _repo.AddAsync(notification, ct);
            var sender = _channelFactory.Resolve(channel);
            await sender.SendAsync(notification, ct);
            notification.MarkAsSent();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Channel dispatch failed for {Id}", notification.Id);
            notification.MarkAsFailed(ex.Message);
        }

        await _repo.UpdateAsync(notification, ct);
        return ToResponse(notification);
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var notification = await GetAndValidateOwnershipAsync(id, userId, ct);
        await _repo.DeleteAsync(notification, ct);
    }

    public async Task<IEnumerable<NotificationResponse>> GetMyNotificationsAsync(Guid userId,
                                                            int page = 1,
                                                            int pageSize = 20,
                                                            CancellationToken ct = default)
    {
        var list = await _repo.GetByUserIdAsync(userId, page, pageSize, ct);

        var result = new List<NotificationResponse>();
        
        foreach (var notification in list)
            result.Add(ToResponse(notification));

        return result;
    }

    public async Task<NotificationResponse> UpdateAsync(Guid Id, UpdateNotificationRequest request, Guid userId, CancellationToken ct = default)
    {
        var notification = await GetAndValidateOwnershipAsync(Id, userId, ct);
        notification.Update(request.Title, request.Content, request.Channel);
        await _repo.UpdateAsync(notification, ct);
        return ToResponse(notification);
    }

    private static NotificationResponse ToResponse(Notification n)
    {
        return new NotificationResponse(
            n.Id,
            n.NotificationId,
            n.UserId,
            n.Title,
            n.Content,
            n.Channel,
            n.Recipient,
            n.CreatedDate,
            n.Status,
            n.FailureReason
        );
    }

    private async Task<Notification> GetAndValidateOwnershipAsync(
                                        Guid id, Guid userId, CancellationToken ct)
    {
        var notification = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Notification), id);

        if (notification.UserId != userId)
            throw new UnauthorizedAccessException();

        return notification;
    }
}