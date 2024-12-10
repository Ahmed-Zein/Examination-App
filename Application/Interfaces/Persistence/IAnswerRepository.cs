using Core.Entities;

namespace Application.Interfaces.Persistence;

public interface IAnswerRepository : IRepository<Answer, int>
{
    Task AddManyAsync(List<Answer> answers, Question question);
}