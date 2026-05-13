namespace NotificationManagement.Domain.Entities;

public class Notification
{
    public string Title { get; set; }
    public string Content { get; set; }
    public ChannelType Channel { get; set; }
}

