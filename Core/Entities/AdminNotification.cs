namespace Core.Entities;

public class AdminNotification : NotificationBase
{
    public NotificationType Level { get; set; } = NotificationType.Information;
    public string Title { get; set; } = string.Empty;
}

public enum NotificationType
{
    Information,
    Warning,
    Error
}