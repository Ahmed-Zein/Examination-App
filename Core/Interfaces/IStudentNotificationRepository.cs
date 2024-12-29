using Core.Entities;
using FluentResults;

namespace Core.Interfaces;

public interface IStudentNotificationRepository
{
    Task AddAsync(StudentNotification studentNotification);
    Task<Result<List<StudentNotification>>> GetAsync(string userId);
    Task<Result> Delete(string studentId, string notificationId);
}