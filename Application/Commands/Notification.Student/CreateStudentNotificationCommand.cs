using Core.Models;
using MediatR;

namespace Application.Commands.Notification.Student;

public record CreateStudentNotificationCommand(StudentNotification Notification) : IRequest;