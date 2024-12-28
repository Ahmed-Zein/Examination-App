using Core.Models;
using FluentResults;

namespace Core.Interfaces;

public interface INotificationRepository
{
    Task<Notification> AddAsync(Notification notification);
    Task<Result<List<Notification>>> GetAsync(string userId);
    Task<Result> Delete(string notificationId);
}