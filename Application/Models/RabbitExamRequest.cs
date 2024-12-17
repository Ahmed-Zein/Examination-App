using Application.DTOs;

namespace Application.Models;

public class RabbitExamRequest
{
    public string StudentId { get; init; } = string.Empty;
    public int ExamId { get; init; }
    public ExamSolutionsDto Solutions { get; init; }
}