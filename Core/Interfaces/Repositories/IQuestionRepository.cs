using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IQuestionRepository : IRepository<Question, int>
{
    Task<List<int>> GetQuestionsIdBySubject(int subjectId);
    Task<List<int>> GetQuestionsIdByExam(int subjectId);
}