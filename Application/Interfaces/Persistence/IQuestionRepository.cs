using Core.Entities;

namespace Application.Interfaces.Persistence;

public interface IQuestionRepository : IRepository<Question, int>
{
    Task<List<int>> GetQuestionsIdBySubject(int subjectId);
    Task<List<int>> GetIdByExam(int subjectId);
    Task<List<Question>> GetBySubjectId(int subjectId);
    Task<List<Question>> GetByExamId(int subjectId);
}