using Application.DTOs;
using FluentResults;

namespace Application.Interfaces;

public interface IQuestionService
{
    Task<List<QuestionDto>> GetBySubject(int subjectId);
    Task<Result<QuestionDto>> GetQuestion(int questionId);
    Task<Result> DeleteQuestion(int questionId);
    Task<Result<List<QuestionDto>>> AddQuestion(List<CreateQuestionDto> questionDtOs, int subjectId);
}