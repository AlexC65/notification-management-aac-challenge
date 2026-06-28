using System.Text.Json;
using Microsoft.Extensions.Logging;
using NotificationManagement.Domain.Entities;
using NotificationManagement.Domain.Enums;
using NotificationManagement.Domain.Exceptions;
using NotificationManagement.Domain.Interfaces;

namespace NotificationManagement.Infrastructure.Channels;

public class PushChannel : INotificationChannel
{
    private readonly ILogger<PushChannel> _logger;

    public ChannelType Channel
    {
        get { return ChannelType.PushNotification; }
    }

    public PushChannel(ILogger<PushChannel> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[Push] Processing notification {Id}", notification.Id);

        // Step 1 — validate device token
        var deviceToken = GetAndValidateDeviceToken(notification);

        // Step 2 — format payload (FCM/APNs compatible structure)
        var payload = FormatPayload(notification, deviceToken);
        _logger.LogInformation("[Push] Payload: {Payload}", payload);

        // Step 3 — simulate send and register state
        await Task.Delay(50, cancellationToken); // replace with FCM/APNs HTTP client

        // simplified PushChannel — consistent with Email and Sms
        _logger.LogInformation("[Push] Delivered to {Token} at {SentAt}", deviceToken, DateTime.UtcNow);
    }

    private static string GetAndValidateDeviceToken(Notification notification)
    {
        // Demo: generate a pseudo-token. Real impl pulls from user device registry.
        var token = $"device-token-{notification.UserId:N}";

        if (string.IsNullOrWhiteSpace(token))
            throw new NotificationChannelException("Push", "Device token is missing or invalid.");

        return token;
    }

    private static string FormatPayload(Notification notification, string deviceToken)
    {
        var payload = new
        {
            to = deviceToken,
            notification = new
            {
                title = notification.Title,
                body = notification.Content.Length > 256
                    ? string.Concat(notification.Content.AsSpan(0, 253), "...")
                    : notification.Content,
            },
            data = new
            {
                notificationId = notification.Id,
                channel = notification.Channel.ToString()
            }
        };

        return JsonSerializer.Serialize(payload);
    }
}