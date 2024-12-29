using Application.Commands.Notification.Student;
using Core.Entities;
using MediatR;

namespace Application.Events.ExamEvaluated.Handlers;

public class ExamEvaluatedNotificationHandler(IMediator mediator) : INotificationHandler<ExamEvaluatedEvent>
{
    public async Task Handle(ExamEvaluatedEvent notification, CancellationToken cancellationToken = default)
    {
        var studentNotification = new StudentNotification
        {
            UserId = notification.StudentId,
            Content = "Your Exam Has been Evaluated"
        };
        await mediator.Send(new CreateStudentNotificationCommand(studentNotification), cancellationToken);
    }
}