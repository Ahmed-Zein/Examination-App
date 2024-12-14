using Application.Interfaces.Persistence;
using Core.Constants;
using Core.Entities;
using FluentResults;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SubjectRepository(AppDbContext context) : ISubjectRepository
{
    public async Task<bool> AnyAsync(int id)
    {
        return await context.Subjects.AnyAsync(subject => subject.Id == id);
    }

    public async Task<List<Subject>> GetAllAsync()
    {
        return await context.Subjects.ToListAsync();
    }

    public async Task<Result<Subject>> GetByIdAsync(int id)
    {
        var subject = await context.Subjects
            .Include(e => e.Questions)
            .ThenInclude(e => e.Answers)
            .Include(e => e.Exams)
            .FirstOrDefaultAsync(e => e.Id == id);

        return subject switch
        {
            null => Result.Fail<Subject>(["Subject not found", ErrorType.NotFound]),
            _ => Result.Ok(subject)
        };
    }

    public void Delete(Subject entity)
    {
        context.Subjects.Remove(entity);
    }

    public async Task AddAsync(Subject entity)
    {
        await context.Subjects.AddAsync(entity);
    }

    public async Task<Result<Subject>> UpdateAsync(int id, Subject toUpdate)
    {
        var subjectResult = await GetByIdAsync(id);
        if (subjectResult.IsFailed)
            return subjectResult;

        var subject = subjectResult.Value;
        subject.Name = toUpdate.Name;

        return Result.Ok(subject);
    }
}