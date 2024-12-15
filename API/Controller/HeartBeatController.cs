using Core.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Route("api/heartbeat")]
[ApiController]
public class HeartBeatController(IRabbitPublisher rabbit) : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok();

    [HttpGet("rabbit")]
    public async Task<IActionResult> GetRabbit()
    {
        var message = new
            { Success = true, list = new List<string>() { "list 1", "list 2" }, number = 1, str = "test str" };

        await rabbit.Publish(message);

        return Ok();
    }
}