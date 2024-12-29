using Core.Entities;
using FluentResults;
using MediatR;

namespace Application.Queries.Notification.Student;

public record GetStudentNotificationsQuery(string StudentId) : IRequest<Result<List<StudentNotification>>>;