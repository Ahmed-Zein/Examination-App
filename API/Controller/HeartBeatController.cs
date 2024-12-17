using Core.Interfaces;
using Infrastructure.Signalr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controller;

[Route("api/heartbeat")]
[ApiController]
public class HeartBeatController(IRabbitPublisher rabbit, IHubContext<ServerNotificationManager, INotificationClient> context)
    : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok();

    [HttpGet("signalr")]
    public IActionResult Notify()
    {
        context.Clients.All.ReceiveNotification("heartbeat");
        return Ok();
    }

    [HttpGet("rabbit")]
    public async Task<IActionResult> GetRabbit()
    {
        var message = new
            { Success = true, list = new List<string>() { "list 1", "list 2" }, number = 1, str = "test str" };

        await rabbit.Publish(message);

        return Ok();
    }
}