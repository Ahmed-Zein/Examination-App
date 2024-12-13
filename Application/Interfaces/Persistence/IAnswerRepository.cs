using Core.Entities;

namespace Application.Interfaces.Persistence;

public interface IAnswerRepository : IRepository<Answer>
{
    Task AddManyAsync(List<Answer> answers, Question question);
}