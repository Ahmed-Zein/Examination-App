using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Core.Models;
using Core.Persistence;
using FluentResults;

namespace Application.Services;

public class ExamResultService(IUnitOfWork unitOfWork, IMapper mapper) : IExamResultService
{
    private readonly IExamResultRepository _examResultRepository = unitOfWork.ExamResultRepository;

    public async Task<Result<PagedData<ExamResultDto>>> GetAllByStudentId(string studentId, PaginationQuery pagination)
    {
        if (!await unitOfWork.StudentRepository.Exists(studentId))
            return Result.Fail("Student not found");

        var repositoryResult = await _examResultRepository.GetByStudentId(studentId, pagination);

        var examResults = mapper.Map<PagedData<ExamResultDto>>(repositoryResult.Value);
        return Result.Ok(examResults);
    }

    public async Task<PagedData<ExamResultDto>> GetAllExamResults(PaginationQuery pagination, SortingQuery sorting)
    {
        var examResults = await _examResultRepository.GetAllAsync(pagination, sorting);
        return mapper.Map<PagedData<ExamResultDto>>(examResults);
    }
}