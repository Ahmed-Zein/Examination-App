using Core.Interfaces;

namespace Infrastructure.Signalr;

public interface IClientNotificationInitiator : IDisposable
{
    Task SendBroadCastNotification(string message);
    Task SendNotificationToUser(string userId);
}

public class ClientNotificationManager(ISignalrClientContext context) : IClientNotificationInitiator
{
    public async Task SendBroadCastNotification(string message)
    {
        if (context.IsConnected())
            await context.InvokeAsync("BroadCast", message);
    }

    public async Task SendNotificationToUser(string userId)
    {
        if (context.IsConnected())
            await context.InvokeAsync("OnExamEvaluation", userId);
    }

    public void Dispose()
    {
        context.TerminateConnection();
    }
}