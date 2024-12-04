namespace Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();
    ISubjectRepository SubjectRepository { get; }
    IStudentRepository StudentRepository { get; }
}