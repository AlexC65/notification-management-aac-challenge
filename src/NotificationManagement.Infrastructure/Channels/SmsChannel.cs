using Microsoft.Extensions.Logging;
using NotificationManagement.Domain.Entities;
using NotificationManagement.Domain.Enums;
using NotificationManagement.Domain.Exceptions;
using NotificationManagement.Domain.Interfaces;

namespace NotificationManagement.Infrastructure.Channels;
// <summary>
/// SMS channel: limits content to 160 characters, logs number and send date.
/// </summary>

public class SmsChannel : INotificationChannel
{
    private const int MaxSmsLength = 160;
    private readonly ILogger<SmsChannel> _logger;
    public ChannelType Channel
    {
        get { return ChannelType.Sms; }
    }
    public SmsChannel(ILogger<SmsChannel> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(Notification notification, CancellationToken ct = default)
    {
        _logger.LogInformation("[SMS] Processing notification {Id}", notification.Id);

        // Step 1 — enforce 160-character limit
        var content = EnforceCharacterLimit(notification.Content);

        // Step 2 — log destination number and timestamp
        // In production: extract phone from user profile or notification metadata.
        var destinationNumber = ExtractPhoneNumber(notification);
        var sentAt = DateTime.UtcNow;
        _logger.LogInformation(
            "[SMS] Sending to {Number} at {SentAt} | Length: {Length} chars | Message: {Content}",
            destinationNumber, sentAt, content.Length, content);

        // Step 3 — simulate send (replace with Twilio/AWS SNS client)
        await Task.Delay(50, ct);

        _logger.LogInformation("[SMS] Delivered to {Number} at {SentAt}", destinationNumber, sentAt);
    }

    private static string EnforceCharacterLimit(string content)
    {
        if (content.Length <= MaxSmsLength)
            return content;

        // Truncate with ellipsis — do NOT throw; SMS just truncates
        return string.Concat(content.AsSpan(0, MaxSmsLength - 3), "...");
    }

    private static string ExtractPhoneNumber(Notification notification)
    {
        if (string.IsNullOrWhiteSpace(notification.Recipient))
            throw new NotificationChannelException("Sms", "Recipient phone number is missing.");

        return notification.Recipient;
    }
}