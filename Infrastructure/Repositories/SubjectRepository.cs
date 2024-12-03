using AutoMapper;
using Core.Entities;
using Core.Repositories;
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
        var subject = await context.Subjects.FindAsync(id);
        return subject is null ? Result.Fail<Subject>("Subject not found") : Result.Ok(subject);
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