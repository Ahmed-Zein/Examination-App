using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using FluentResults;

namespace Application.Services;

public class ExamResultService(IUnitOfWork unitOfWork, IMapper mapper) : IExamResultService
{
    private readonly IExamResultRepository _examResultRepository = unitOfWork.ExamResultRepository;

    public async Task<Result<List<ExamResultDto>>> GetAllByStudentId(string studentId)
    {
        if (!await unitOfWork.StudentRepository.Exists(studentId))
            return Result.Fail("Student not found");

        var repositoryResult = await _examResultRepository.GetByStudentId(studentId);

        var examResults = mapper.Map<List<ExamResultDto>>(repositoryResult.Value);
        return Result.Ok(examResults);
    }

    public async Task<List<ExamResultDto>> GetAllExamResults()
    {
        var examResults = await _examResultRepository.GetAllAsync();
        return mapper.Map<List<ExamResultDto>>(examResults);
    }
}