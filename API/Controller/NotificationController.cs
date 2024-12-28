using API.Helper;
using Application.Models;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Route("api/")]
[ApiController]
public class NotificationController(
    IStudentNotificationRepository studentRepository,
    IAdminNotificationRepository adminRepository) : ControllerBase
{
    [HttpGet("students/{studentId}/notifications")]
    public async Task<ActionResult<JsonResponse<IEnumerable<StudentNotification>>>> GetStudentNotifications(
        string studentId)
    {
        var notificationsResult = await studentRepository.GetAsync(studentId);

        return notificationsResult.IsSuccess
            ? Ok(JsonResponse<IEnumerable<StudentNotification>>.Ok(notificationsResult.Value))
            : ApiResponseHelper.HandelError(notificationsResult.Errors);
    }

    [HttpDelete("students/{studentId}/notifications/{notificationId}")]
    public async Task<IActionResult> DeleteStudentNotification(string studentId, string notificationId)
    {
        var notificationsResult = await studentRepository.Delete(notificationId);

        return notificationsResult.IsSuccess
            ? NoContent()
            : ApiResponseHelper.HandelError(notificationsResult.Errors);
    }

    [HttpDelete("admins/notifications")]
    public async Task<IActionResult> GetAdminNotifications()
    {
        var notifications = await adminRepository.GetAllAsync();
        return Ok(JsonResponse<IEnumerable<AdminNotification>>.Ok(notifications));
    }
}