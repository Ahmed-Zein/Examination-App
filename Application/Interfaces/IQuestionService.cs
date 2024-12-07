using Application.DTOs;
using FluentResults;

namespace Application.Interfaces;

public interface IQuestionService
{
    Task<Result<List<QuestionDto>>> AddQuestion(List<CreateQuestionDto> quetionsDto, int subjectId);
    Task<Result<QuestionDto>> GetQuestion(int questionId);
    Task<List<QuestionDto>> GetBySubject(int subjectId);
}