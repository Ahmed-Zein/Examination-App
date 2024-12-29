namespace Core.Interfaces;

public interface IClientSideNotification : IDisposable
{
    Task SendBroadCastNotification(string message);
    Task SendNotificationToUser(string userId);
}