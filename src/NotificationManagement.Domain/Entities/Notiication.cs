namespace NotificationManagement.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public ChannelType Channel { get; set; }
    public DateTime CreatedDate { get; set; }

public Notification(string title, string content, ChannelType channel)
{
    if (string.IsNullOrEmpty(title))
        throw new ArgumentException("Title cannot be null or empty.", nameof(title));
    if (string.IsNullOrEmpty(content))
        throw new ArgumentException("Content cannot be null or empty.", nameof(content));

    Id = Guid.NewGuid();
    Title = title;
    Content = content;
    Channel = channel;
    CreatedDate = DateTime.UtcNow;
}

}

