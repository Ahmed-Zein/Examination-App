using Core.Constants;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Signalr;

[Authorize]
public class ServerNotificationManager : Hub<INotificationClient>, IServerNotificationManager
{
    private const string AdminGroupName = AuthRolesConstants.Admin;

    public override async Task OnConnectedAsync()
    {
        await Clients.Client(Context.ConnectionId).ReceiveNotification("Welcome on Board");
        if (Context.User?.IsInRole(AuthRolesConstants.Admin) ?? true)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, AdminGroupName);
        }
    }

    public async Task OnExamEvaluation(string userId)
    {
        await Clients.User(userId).ReceiveNotification("Your exam evaluation has been completed.", 1);
    }


    public async Task BroadCast(string message)
    {
        await Clients.All.ReceiveNotification(message);
    }
}

public interface INotificationClient
{
    Task ReceiveNotification(string message, int severity = 0);
}