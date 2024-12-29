using Core.Entities;
using Core.Interfaces;
using FluentResults;
using MediatR;

namespace Application.Queries.Notification.Student.Handlers;

public class
    GetStudentNotificationsQueryHandler(IStudentNotificationRepository studentRepository) :
    IRequestHandler<GetStudentNotificationsQuery, Result<List<StudentNotification>>>
{
    public async Task<Result<List<StudentNotification>>> Handle(GetStudentNotificationsQuery request,
        CancellationToken cancellationToken = default)
    {
        return await studentRepository.GetAsync(request.StudentId);
    }
}