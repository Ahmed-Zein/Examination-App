using Core.Interfaces;

namespace Infrastructure.Signalr;

public class IClientSideNotificationManager(ISignalrClientContext context) : IClientSideNotification
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