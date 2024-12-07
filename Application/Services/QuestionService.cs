using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Persistence;
using Application.Validators;
using AutoMapper;
using Core.Entities;
using FluentResults;

namespace Application.Services;

public class QuestionService(IUnitOfWork unitOfWork, IMapper mapper) : IQuestionService
{
    private readonly IQuestionRepository _questionRepository = unitOfWork.QuestionRepository;

    public async Task<Result<QuestionDto>> AddQuestion(CreateQuestionDto questionDto, int subjectId)
    {
        var validator = new CreateQuestionDtoValidator();
        var result = await validator.ValidateAsync(questionDto);

        if (!result.IsValid)
            return Result.Fail(result.Errors.Select(e => e.ErrorMessage));

        var subjectExists = await unitOfWork.SubjectRepository.AnyAsync(subjectId);
        if (!subjectExists)
            return Result.Fail<QuestionDto>("Subject not found");

        var question = mapper.Map<Question>(questionDto);
        question.SubjectId = subjectId;

        await _questionRepository.AddAsync(question);
        await unitOfWork.CommitAsync();

        return Result.Ok(mapper.Map<QuestionDto>(question));
    }

    public async Task<Result<QuestionDto>> GetQuestion(int questionId)
    {
        var questionResult = await _questionRepository.GetByIdAsync(questionId);
        return questionResult switch
        {
            { IsSuccess: true } => Result.Ok(mapper.Map<QuestionDto>(questionResult.Value)),
            { IsSuccess: false } => questionResult.ToResult()
        };
    }

    public async Task<List<QuestionDto>> GetBySubject(int subjectId)
    {
        var questions = await _questionRepository.GetBySubjectId(subjectId);
        return mapper.Map<List<QuestionDto>>(questions);
    }
}