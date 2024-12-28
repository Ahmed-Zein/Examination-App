using Core.Models;
using FluentResults;
using MediatR;

namespace Application.Queries.Notification.Student;

public record GetStudentNotificationsQuery(string StudentId) : IRequest<Result<List<StudentNotification>>>;