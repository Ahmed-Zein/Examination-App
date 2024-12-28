using Core.Models;
using FluentResults;

namespace Core.Interfaces;

public interface IStudentNotificationRepository
{
    Task<StudentNotification> AddAsync(StudentNotification studentNotification);
    Task<Result<List<StudentNotification>>> GetAsync(string userId);
    Task<Result> Delete(string notificationId);
}