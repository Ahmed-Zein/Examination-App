using Application.DTOs;
using FluentResults;

namespace Application.Interfaces;

public interface IExamResultService
{
    Task<Result<List<ExamResultDto>>> GetAllByStudentId(string studentId);
    Task<List<ExamResultDto>> GetAllExamResults();
}