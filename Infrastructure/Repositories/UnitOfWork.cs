using Core.Interfaces.Repositories;
using Core.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private ISubjectRepository? _subjectRepository;
    private IStudentRepository? _studentRepository = new StudentRepository(context);

    public ISubjectRepository SubjectRepository => _subjectRepository ??= new SubjectRepository(context);

    public IStudentRepository StudentRepository => _studentRepository ??= new StudentRepository(context);
}