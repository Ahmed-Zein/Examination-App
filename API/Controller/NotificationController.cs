using API.Helper;
using Application.Models;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Route("api/users/{userId}/notifications")]
[ApiController]
public class NotificationController(INotificationRepository repository) : ControllerBase
{
    // TODO: DELETE this  
    [HttpPost]
    public async Task<ActionResult<JsonResponse<Notification>>> Create(string userId)
    {
        var notification = new Notification
        {
            UserId = userId,
            CreatedAt = DateTime.Now,
            Content = "Created notification"
        };
        await repository.AddAsync(notification);
        return Ok(JsonResponse<Notification>.Ok(notification));
    }

    [HttpGet]
    public async Task<ActionResult<JsonResponse<IEnumerable<Notification>>>> Get(string userId)
    {
        var notificationsResult = await repository.GetAsync(userId);

        return notificationsResult.IsSuccess
            ? Ok(JsonResponse<IEnumerable<Notification>>.Ok(notificationsResult.Value))
            : ApiResponseHelper.HandelError(notificationsResult.Errors);
    }

    [HttpDelete("{notificationId}")]
    public async Task<IActionResult> Delete(string userId, string notificationId)
    {
        var notificationsResult = await repository.Delete(notificationId);

        return notificationsResult.IsSuccess
            ? NoContent()
            : ApiResponseHelper.HandelError(notificationsResult.Errors);
    }
}