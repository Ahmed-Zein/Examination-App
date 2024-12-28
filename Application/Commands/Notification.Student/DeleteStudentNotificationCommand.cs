using FluentResults;
using MediatR;

namespace Application.Commands.Notification.Student;

public record DeleteStudentNotificationCommand(string StudentId, string NotificationId) : IRequest<Result>;