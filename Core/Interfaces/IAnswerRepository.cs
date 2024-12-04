using Core.Entities;
using Core.Interfaces.Repositories;

namespace Core.Interfaces;

public interface IAnswerRepository : IRepository<Answer, int>
{
    Task AddManyAsync(List<Answer> answers, Question question);
}