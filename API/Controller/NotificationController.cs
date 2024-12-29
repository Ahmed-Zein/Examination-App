using API.Helper;
using Application.Commands.Notification.Student;
using Application.Models;
using Application.Queries.Notification.Admin;
using Application.Queries.Notification.Student;
using Core.Constants;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Authorize]
[ApiController]
[Route("api/")]
public class NotificationController(IMediator mediator) : ControllerBase
{
    [HttpGet("students/{studentId}/notifications")]
    public async Task<ActionResult<JsonResponse<IEnumerable<StudentNotification>>>> GetStudentNotifications(
        string studentId)
    {
        var notificationsResult = await mediator.Send(new GetStudentNotificationsQuery(studentId));

        return notificationsResult.IsSuccess
            ? Ok(JsonResponse<IEnumerable<StudentNotification>>.Ok(notificationsResult.Value))
            : ApiResponseHelper.HandelError(notificationsResult.Errors);
    }

    [HttpDelete("students/{studentId}/notifications/{notificationId}")]
    public async Task<IActionResult> DeleteStudentNotification(string studentId, string notificationId)
    {
        var notificationsResult = await mediator.Send(new DeleteStudentNotificationCommand(studentId, notificationId));

        return notificationsResult.IsSuccess
            ? NoContent()
            : ApiResponseHelper.HandelError(notificationsResult.Errors);
    }

    [HttpGet("admins/notifications")]
    [Authorize(Roles = AuthRolesConstants.Admin)]
    public async Task<IActionResult> GetAdminNotifications()
    {
        var notifications = await mediator.Send(new GetAdminNotificationsQuery());
        return Ok(JsonResponse<IEnumerable<AdminNotification>>.Ok(notifications));
    }
}