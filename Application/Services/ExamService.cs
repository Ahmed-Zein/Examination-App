using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Persistence;
using Application.Validators;
using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Enums;
using FluentResults;

namespace Application.Services;

public class ExamService(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    CreateExamDtoValidator createExamDtoValidator,
    AddQuestionToExamValidator questionToExamValidator) : IExamService
{
    private readonly IExamRepository _examRepository = unitOfWork.ExamRepository;

    public async Task<Result<ExamDto>> CreateExam(CreateExamDto createExamDto, int subjectId)
    {
        var validationResult = await createExamDtoValidator.ValidateAsync(createExamDto);
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.Errors.Select(item => item.ErrorMessage).Append(ErrorType.BadRequest));

        var exam = mapper.Map<Exam>(createExamDto);
        exam.SubjectId = subjectId;

        var subjectExist = await unitOfWork.SubjectRepository.AnyAsync(subjectId);
        if (!subjectExist)
            return Result.Fail<ExamDto>("Subject not found");

        await _examRepository.AddAsync(exam);

        await unitOfWork.CommitAsync();

        return Result.Ok(mapper.Map<ExamDto>(exam));
    }

    public async Task<Result<ExamDto>> GetExamById(int id)
    {
        var examResult = await _examRepository.GetByIdAsync(id);
        return examResult.IsSuccess
            ? Result.Ok(mapper.Map<ExamDto>(examResult.Value))
            : examResult.ToResult();
    }

    public async Task<Result<List<ExamDto>>> GetExams(int subjectId)
    {
        var examsResult = await _examRepository.GetAllBySubject(subjectId);
        if (!examsResult.IsSuccess)
            return examsResult.ToResult();
        return mapper.Map<List<ExamDto>>(examsResult.Value);
    }


    public async Task<Result<StudentExam>> PullExam(string userId, int subjectId)
    {
        var examIds = await _examRepository.GetAllExamIds(subjectId);
        if (examIds.Count == 0)
            return Result.Fail(["No exams found", ErrorType.NotFound]);

        var randomIndex = new Random().Next(0, examIds.Count);

        var exam = (await _examRepository.GetByIdAsync(examIds[randomIndex])).Value;
        var examResult = new ExamResult
        {
            ExamId = exam.Id,
            AppUserId = userId,
            StartTime = DateTime.UtcNow,
            TotalScore = exam.Questions.Count,
            Status = ExamResultStatus.UnSubmitted
        };
        await unitOfWork.ExamResultRepository.AddAsync(examResult);
        await unitOfWork.CommitAsync();

        var studentExamDto = mapper.Map<StudentExam>(exam);
        studentExamDto.ExamResultId = examResult.Id;

        return Result.Ok(studentExamDto);
    }

    public async Task<Result> UpdateExamQuestions(AddQuestionToExamDto questionDto)
    {
        var validationResult = await questionToExamValidator.ValidateAsync(questionDto);
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.Errors.Select(error => error.ErrorMessage));

        if (!await _examRepository.ExamExistsForSubject(questionDto.SubjectId, questionDto.ExamId))
            return Result.Fail("The exam doesn't exists for the given subject");

        await _examRepository.UpdateExamQuestions(questionDto.ExamId, questionDto.QuestionIds);
        await unitOfWork.CommitAsync();

        return Result.Ok();
    }

    public async Task<Result> DeleteExam(int examId)
    {
        var repositoryResult = await _examRepository.GetByIdAsync(examId);
        if (!repositoryResult.IsSuccess)
            return repositoryResult.ToResult();

        if (await _examRepository.HasExamResults(examId))
            return Result.Fail(["Cannot delete an exam that has  Results", ErrorType.BadRequest]);

        _examRepository.Delete(repositoryResult.Value);
        await unitOfWork.CommitAsync();
        return Result.Ok();
    }

    public async Task<Result<ExamDto>> UpdateExam(int examId, UpdateExamDto examDto)
    {
        var repositoryResult = await _examRepository.UpdateAsync(examId, mapper.Map<Exam>(examDto));
        if (!repositoryResult.IsSuccess)
            return repositoryResult.ToResult();

        await unitOfWork.CommitAsync();
        return Result.Ok(mapper.Map<ExamDto>(repositoryResult.Value));
    }
}