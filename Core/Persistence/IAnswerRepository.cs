using Core.Entities;

namespace Core.Persistence;

public interface IAnswerRepository : IRepository<Answer>
{
    Task AddManyAsync(List<Answer> answers, Question question);
}