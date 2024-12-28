using Core.Constants;
using Core.Interfaces;
using Core.Models;
using FluentResults;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class NotificationRepository(NotificationDbContext context) : INotificationRepository
{
    private const int MaxNotificationCount = 20;
    private readonly DateTime _notificationCutOffDate = DateTime.Now - TimeSpan.FromDays(7);


    public async Task<Notification> AddAsync(Notification notification)
    {
        await context.Notifications.InsertOneAsync(notification);
        return notification;
    }

    public async Task<Result<List<Notification>>> GetAsync(string userId)
    {
        var filter = Builders<Notification>.Filter.And(
            Builders<Notification>.Filter.Eq(n => n.UserId, userId),
            Builders<Notification>.Filter.Gt(n => n.CreatedAt, _notificationCutOffDate)
        );
        var notifications = await context.Notifications
            .Find(filter)
            .Limit(MaxNotificationCount)
            .ToListAsync();

        return notifications is not null
            ? notifications
            : Result.Fail(["Notification not found", ErrorType.NotFound]);
    }

    public async Task<Result> Delete(string notificationId)
    {
        var deleted =
            await context.Notifications.DeleteOneAsync(Builders<Notification>.Filter.Eq(n => n.Id, notificationId));

        return deleted is not null && deleted.IsAcknowledged
            ? Result.Ok()
            : Result.Fail(["Notification not found", ErrorType.NotFound]);
    }
}