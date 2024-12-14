using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Persistence;
using Application.Validators;
using AutoMapper;
using Core.Constants;
using Core.Entities;
using FluentResults;
using FluentValidation;

namespace Application.Services;

public class QuestionService(IUnitOfWork unitOfWork, IMapper mapper) : IQuestionService
{
    private readonly IQuestionRepository _questionRepository = unitOfWork.QuestionRepository;

    public async Task<Result> DeleteQuestion(int questionId)
    {
        var repositoryResult = await _questionRepository.GetByIdAsync(questionId);
        if (!repositoryResult.IsSuccess)
            return repositoryResult.ToResult();
        _questionRepository.Delete(repositoryResult.Value);
        await unitOfWork.CommitAsync();
        return Result.Ok();
    }

    public async Task<Result<List<QuestionDto>>> AddQuestion(List<CreateQuestionDto> questionDtOs, int subjectId)
    {
        var validator = new CreateQuestionListDtoValidator();
        var result = await validator.ValidateAsync(questionDtOs);

        if (!result.IsValid)
            return Result.Fail(result.Errors.Select(e => e.ErrorMessage).Append(ErrorType.BadRequest));

        var subjectExists = await unitOfWork.SubjectRepository.AnyAsync(subjectId);
        if (!subjectExists)
            return Result.Fail(["Subject not found", ErrorType.NotFound]);

        var questions = mapper.Map<List<Question>>(questionDtOs);
        foreach (var question in questions)
        {
            question.SubjectId = subjectId;
            await _questionRepository.AddAsync(question);
        }

        await unitOfWork.CommitAsync();

        return Result.Ok(mapper.Map<List<QuestionDto>>(questions));
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