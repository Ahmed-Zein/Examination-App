using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Persistence;
using Application.Validators;
using AutoMapper;
using Core.Entities;
using FluentResults;

namespace Application.Services;

public class ExamService(IUnitOfWork unitOfWork, AddQuestionToExamValidator questionToExamValidator, IMapper mapper)
    : IExamService
{
    private readonly IExamRepository _examRepository = unitOfWork.ExamRepository;

    public async Task<Result<ExamDto>> CreateExam(CreateExamDto createExamDto, int subjectId)
    {
        var validator = new CreateExamDtoValidator();
        var validationResult = await validator.ValidateAsync(createExamDto);
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.Errors.Select(item => item.ErrorMessage));

        var exam = mapper.Map<Exam>(createExamDto);

        var examSubjectResult = await unitOfWork.SubjectRepository.GetByIdAsync(subjectId);
        if (!examSubjectResult.IsSuccess)
            return examSubjectResult.ToResult();

        var subject = examSubjectResult.Value;
        subject.Exams.Add(exam);

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
            return Result.Fail(examsResult.Errors);
        return mapper.Map<List<ExamDto>>(examsResult.Value);
    }

    public async Task<Result<ExamDto>> GetRandomExam(int subjectId)
    {
        var examIds = await _examRepository.GetAllExamIds(subjectId);
        if (examIds.Count == 0)
            return Result.Fail("No exams found");
        var randomIndex = new Random().Next(0, examIds.Count);

        var examsResult = await _examRepository.GetByIdAsync(examIds[randomIndex]);
        return Result.Ok(mapper.Map<ExamDto>(examsResult.Value));
    }

    public async Task<Result> AddQuestionToExam(AddQuestionToExamDto questionDto)
    {
        var validationResult = await questionToExamValidator.ValidateAsync(questionDto);
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.Errors.Select(error => error.ErrorMessage));

        if (!await _examRepository.ExamExistsForSubject(questionDto.SubjectId, questionDto.ExamId))
        {
            return Result.Fail("The exam doesn't exists for the given subject");
        }

        await _examRepository.AddQuestionsToExam(questionDto.ExamId, questionDto.QuestionIds);
        await unitOfWork.CommitAsync();

        return Result.Ok();
    }
}