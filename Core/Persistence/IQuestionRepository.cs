using Core.Entities;

namespace Core.Persistence;

public interface IQuestionRepository : IRepository<Question>
{
    Task<List<int>> GetQuestionsIdBySubject(int subjectId);
    Task<List<int>> GetIdByExam(int subjectId);
    Task<List<Question>> GetBySubjectId(int subjectId);
    Task<List<Question>> GetByExamId(int subjectId);
}