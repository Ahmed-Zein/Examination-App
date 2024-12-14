using Core.Entities;
using FluentResults;

namespace Application.Interfaces.Persistence;

public interface IExamRepository : IRepository<Exam>
{
    Task<Result<List<Exam>>> GetAllBySubject(int subjectId);
    Task<List<int>> GetAllExamIds(int subjectId);
    Task UpdateExamQuestions(int examId, List<int> newQuestions);
    Task<bool> ExamExistsForSubject(int subjectId, int examId);
    Task<bool> HasExamResults(int examId);
}