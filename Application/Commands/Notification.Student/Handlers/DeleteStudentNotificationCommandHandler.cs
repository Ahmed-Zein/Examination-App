using Core.Interfaces;
using FluentResults;
using MediatR;

namespace Application.Commands.Notification.Student.Handlers;

public class DeleteStudentNotificationCommandHandler(IStudentNotificationRepository studentNotificationRepository)
    : IRequestHandler<DeleteStudentNotificationCommand, Result>
{
    public async Task<Result> Handle(DeleteStudentNotificationCommand request, CancellationToken cancellationToken)
    {
        return await studentNotificationRepository.Delete(request.StudentId, request.NotificationId);
    }
}