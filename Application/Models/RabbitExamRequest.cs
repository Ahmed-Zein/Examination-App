using Application.DTOs;

namespace Application.Models;

public class RabbitExamRequest
{
    public int ExamId { get; init; }
    public ExamSolutionsDto Solutions { get; init; }
}