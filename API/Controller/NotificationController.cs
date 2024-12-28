using API.Helper;
using Application.Commands.Notification.Student;
using Application.Models;
using Application.Queries.Notification.Student;
using Core.Interfaces;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[ApiController]
[Route("api/")]
public class NotificationController(
    IMediator mediator,
    IAdminNotificationRepository adminRepository) : ControllerBase
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
    public async Task<IActionResult> GetAdminNotifications()
    {
        var notifications = await adminRepository.GetAllAsync();
        return Ok(JsonResponse<IEnumerable<AdminNotification>>.Ok(notifications));
    }
}