namespace Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();
    ISubjectRepository SubjectRepository { get; }
    IStudentRepository StudentRepository { get; }
    IQuestionRepository QuestionRepository { get; }
    IAnswerRepository AnswerRepository { get; }
}