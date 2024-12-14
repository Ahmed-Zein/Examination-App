using Application.DTOs;
using Application.Models;
using FluentResults;

namespace Application.Interfaces;

public interface IExamResultService
{
    Task<Result<PagedData<ExamResultDto>>> GetAllByStudentId(string studentId, PaginationQuery pagination);
    Task<PagedData<ExamResultDto>> GetAllExamResults(PaginationQuery pagination);
}