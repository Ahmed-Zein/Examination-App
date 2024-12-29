using MediatR;

namespace Application.Events.ExamEvaluated;

public record ExamEvaluatedEvent(string StudentId) : INotification;