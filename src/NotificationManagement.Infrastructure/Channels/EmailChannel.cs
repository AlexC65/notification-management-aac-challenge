using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using NotificationManagement.Domain.Entities;
using NotificationManagement.Domain.Enums;
using NotificationManagement.Domain.Exceptions;
using NotificationManagement.Domain.Interfaces;

/// <summary>
/// Email channel: validates recipient format, generates a template, logs the send.
/// </summary>
namespace NotificationManagement.Infrastructure.Channels;

public class EmailChannel : INotificationChannel
{
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    private readonly ILogger<EmailChannel> _logger;

    public ChannelType Channel
    {
        get { return ChannelType.Email; }
    }

    public EmailChannel(ILogger<EmailChannel> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(Notification notification, CancellationToken ct = default)
    {
        _logger.LogInformation("[Email] Processing notification {Id}", notification.Id);

        // Step 1 — validate recipient format (email embedded in title for demo purposes)
        ValidateRecipient(notification);

        // Step 2 — generate template
        var template = GenerateTemplate(notification);
        _logger.LogInformation("[Email] Template generated:\n{Template}", template);

        // Step 3 — simulate send (replace with SMTP/SendGrid/SES client)
        await Task.Delay(50, ct); // simulate I/O

        _logger.LogInformation("[Email] Sent at {Timestamp}", DateTime.UtcNow);
    }

    private static void ValidateRecipient(Notification notification)
    {
        // For the demo we check that the title isn't empty;
        // in production this would validate notification.RecipientEmail.
        if (string.IsNullOrWhiteSpace(notification.Title))
            throw new NotificationChannelException("Email", "Recipient title is empty.");

        // Example: validate a hypothetical embedded email address pattern
        var emailPattern = EmailRegex;
        if (!emailPattern.IsMatch(notification.Content) && notification.Content.Contains('@'))
            throw new NotificationChannelException("Email", "Invalid recipient email format.");
    }

    private static string GenerateTemplate(Notification notification)
    {
        return $"""
               ============================================
               Subject : {notification.Title}
               Date    : {DateTime.UtcNow:R}
               ============================================
               {notification.Content}
               ============================================
               """;
    }
}