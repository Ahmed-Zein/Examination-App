using Core.Entities;
using FluentResults;

namespace Application.Interfaces.Persistence;

public interface IExamResultRepository : IRepository<ExamResult, int>
{
    Task<Result<List<ExamResult>>> GetByStudentId(string studentId);
}