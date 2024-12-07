using Core.Entities;

namespace Application.Interfaces.Persistence;

public interface IQuestionRepository : IRepository<Question, int>
{
    Task<List<int>> GetQuestionsIdBySubject(int subjectId);
    Task<List<int>> GetQuestionsIdByExam(int subjectId);
}