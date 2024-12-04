using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context, UserManager<AppUser> userManager) : IUnitOfWork
{
    private ISubjectRepository? _subjectRepository;
    private IStudentRepository? _studentRepository;
    private IQuestionRepository? _questionRepository;
    private IAnswerRepository? _answerRepository;

    public ISubjectRepository SubjectRepository => _subjectRepository ??= new SubjectRepository(context);
    public IStudentRepository StudentRepository => _studentRepository ??= new StudentRepository(userManager);
    public IQuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(context);
    public IAnswerRepository AnswerRepository => _answerRepository ??= new AnswerRepository(context);

    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
        userManager.Dispose();
    }
}