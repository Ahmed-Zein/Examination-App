using Core.Entities;
using Core.Models;
using FluentResults;

namespace Core.Persistence;

public interface IExamResultRepository : IRepository<ExamResult>
{
    Task<Result<PagedData<ExamResult>>> GetByStudentId(string studentId, PaginationQuery pagination);
    Task<PagedData<ExamResult>> GetAllAsync(PaginationQuery pagination, SortingQuery sorting);
}