using Core.Entities;
using MediatR;

namespace Application.Commands.Notification.Student;

public record CreateStudentNotificationCommand(StudentNotification Notification) : IRequest;