using NotificationManagement.Domain.Enums;

namespace NotificationManagement.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public int? NotificationId { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public ChannelType Channel { get; private set; }
    public string? Recipient { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public NotificationStatus Status { get; private set; }
    public string? FailureReason { get; private set; }

    private Notification()
    {
    }

    public static Notification Create(Guid userId, string title, string content,
                                                ChannelType channel, string recipient)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        if (string.IsNullOrEmpty(title))
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));
        if (string.IsNullOrEmpty(content))
            throw new ArgumentException("Content cannot be null or empty.", nameof(content));
        if (string.IsNullOrEmpty(recipient))
            throw new ArgumentException("Recipient cannot be null or empty.", nameof(recipient));

        return new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Content = content,
            Channel = channel,
            CreatedDate = DateTime.UtcNow,
            Recipient = recipient
        };
    }
    public void Update(string title, string content, ChannelType channel)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty.", nameof(content));

        Title = title;
        Content = content;
        Channel = channel;
    }
    public void MarkAsSent()
    {
        Status = NotificationStatus.Sent;
        CreatedDate = DateTime.UtcNow;
    }
    public void MarkAsFailed(string reason)
    {
        Status = NotificationStatus.Failed;
        FailureReason = reason;
    }

    public void SetSequenceNumber(int number)
    {
        if (NotificationId != 0)
            throw new InvalidOperationException("Sequence number already set.");
        NotificationId = number;
    }
}

