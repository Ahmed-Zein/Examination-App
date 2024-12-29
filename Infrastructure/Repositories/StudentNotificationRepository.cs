using Core.Constants;
using Core.Entities;
using Core.Interfaces;
using FluentResults;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class StudentNotificationRepository(NotificationDbContext context) : IStudentNotificationRepository
{
    private const int MaxNotificationCount = 20;
    private readonly DateTime _notificationCutOffDate = DateTime.Now - TimeSpan.FromDays(7);


    public async Task AddAsync(StudentNotification studentNotification)
    {
        await context.StudentNotifications.InsertOneAsync(studentNotification);
    }

    public async Task<Result<List<StudentNotification>>> GetAsync(string userId)
    {
        var filter = Builders<StudentNotification>.Filter.And(
            Builders<StudentNotification>.Filter.Eq(n => n.UserId, userId),
            Builders<StudentNotification>.Filter.Gt(n => n.CreatedAt, _notificationCutOffDate)
        );
        var notifications = await context.StudentNotifications
            .Find(filter)
            .Limit(MaxNotificationCount)
            .ToListAsync();

        return notifications is not null
            ? notifications
            : Result.Fail(["Notification not found", ErrorType.NotFound]);
    }

    public async Task<Result> Delete(string studentId, string notificationId)
    {
        var deleted =
            await context.StudentNotifications.DeleteOneAsync(
                Builders<StudentNotification>.Filter.And(
                    Builders<StudentNotification>.Filter.Eq(n => n.UserId, studentId),
                    Builders<StudentNotification>.Filter.Eq(n => n.Id, notificationId))
            );

        return deleted is not null && deleted.IsAcknowledged
            ? Result.Ok()
            : Result.Fail(["Notification not found", ErrorType.NotFound]);
    }
}