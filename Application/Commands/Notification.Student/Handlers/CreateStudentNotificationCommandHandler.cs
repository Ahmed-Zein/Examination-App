using Core.Interfaces;
using MediatR;

namespace Application.Commands.Notification.Student.Handlers;

public class CreateStudentNotificationCommandHandler(IStudentNotificationRepository notificationRepository)
    : IRequestHandler<CreateStudentNotificationCommand>
{
    public async Task Handle(CreateStudentNotificationCommand request, CancellationToken cancellationToken = default)
    {
        await notificationRepository.AddAsync(request.Notification);
    }
}