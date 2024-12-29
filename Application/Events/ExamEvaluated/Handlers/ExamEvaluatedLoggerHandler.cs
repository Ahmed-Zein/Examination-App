using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.ExamEvaluated.Handlers;

public class ExamEvaluatedLoggerHandler(ILogger<ExamEvaluatedLoggerHandler> logger)
    : INotificationHandler<ExamEvaluatedEvent>
{
    public Task Handle(ExamEvaluatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("New Exam Evaluated for student {studentId} at {timestamp}", notification.StudentId,
            DateTime.UtcNow);
        return Task.CompletedTask;
    }
}