using Application.Models;
using Core.Entities;
using Core.Models;
using FluentResults;

namespace Application.Interfaces.Persistence;

public interface IExamResultRepository : IRepository<ExamResult>
{
    Task<Result<PagedData<ExamResult>>> GetByStudentId(string studentId, PaginationQuery pagination);
    Task<PagedData<ExamResult>> GetAllAsync(PaginationQuery query, SortingQuery sorting);
}