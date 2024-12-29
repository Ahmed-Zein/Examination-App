using Core.Interfaces;
using MediatR;

namespace Application.Events.ExamEvaluated.Handlers;

public class ExamEvaluatedSignalrHandler(IClientSideNotification clientSideNotification)
    : INotificationHandler<ExamEvaluatedEvent>
{
    public async Task Handle(ExamEvaluatedEvent notification, CancellationToken cancellationToken)
    {
        await clientSideNotification.SendNotificationToUser(notification.StudentId);
    }
}