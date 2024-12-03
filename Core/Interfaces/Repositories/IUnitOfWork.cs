using Core.Repositories;

namespace Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    ISubjectRepository SubjectRepository { get; }
    IStudentRepository StudentRepository { get; }
}