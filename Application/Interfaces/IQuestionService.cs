using Application.DTOs;
using FluentResults;

namespace Application.Services;

public interface IQuestionService
{
    Task<Result<QuestionDto>> AddQuestion(CreateQuestionDto questionDto, int subjectId);
    Task<Result<QuestionDto>> GetQuestion(int questionId);
}