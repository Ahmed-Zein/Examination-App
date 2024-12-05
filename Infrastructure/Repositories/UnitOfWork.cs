using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context, UserManager<AppUser> userManager) : IUnitOfWork
{
    private IExamRepository? _examRepository;
    private IAnswerRepository? _answerRepository;
    private ISubjectRepository? _subjectRepository;
    private IStudentRepository? _studentRepository;
    private IQuestionRepository? _questionRepository;

    public IExamRepository ExamRepository => _examRepository ??= new ExamRepository(context);
    public IAnswerRepository AnswerRepository => _answerRepository ??= new AnswerRepository(context);
    public ISubjectRepository SubjectRepository => _subjectRepository ??= new SubjectRepository(context);
    public IQuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(context);
    public IStudentRepository StudentRepository => _studentRepository ??= new StudentRepository(userManager);

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