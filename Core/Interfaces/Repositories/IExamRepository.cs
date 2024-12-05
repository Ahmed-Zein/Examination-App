using Core.Entities;
using FluentResults;

namespace Core.Interfaces.Repositories;

public interface IExamRepository : IRepository<Exam, int>
{
    Task<Result<List<Exam>>> GetAllBySubject(int subjectId);
    Task<List<int>> GetAllExamIds(int subjectId);
}