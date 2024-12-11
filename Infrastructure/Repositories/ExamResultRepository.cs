using Application.Interfaces.Persistence;
using Core.Entities;
using FluentResults;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ExamResultRepository(AppDbContext context) : IExamResultRepository
{
    public Task<bool> AnyAsync(int id)
    {
        return context.ExamResults.AnyAsync(e => e.Id == id);
    }

    public async Task<List<ExamResult>> GetAllAsync()
    {
        return await context.ExamResults.ToListAsync();
    }

    public async Task<Result<ExamResult>> GetByIdAsync(int id)
    {
        var exam = await context.ExamResults.Include(e => e.Exam)
            .FirstOrDefaultAsync(e => e.Id == id);
        return exam is not null ? Result.Ok(exam) : Result.Fail("exam not found");
    }

    public void Delete(ExamResult entity)
    {
        context.ExamResults.Remove(entity);
    }

    public async Task AddAsync(ExamResult entity)
    {
        await context.ExamResults.AddAsync(entity);
    }

    public async Task<Result<ExamResult>> UpdateAsync(int id, ExamResult toUpdate)
    {
        var examResult = await context.ExamResults.FirstOrDefaultAsync(e => e.Id == id);
        if (examResult is null)
            return Result.Fail("exam result not found");

        var examDurationTask = context.Exams.Where(e => e.Id == examResult.ExamId)
            .Select(e => e.Duration)
            .FirstOrDefaultAsync();

        examResult.Status = toUpdate.Status;
        examResult.EndTime = toUpdate.EndTime;

        var examDuration = await examDurationTask;
        if (examResult.StartTime + examDuration < toUpdate.EndTime)
        {
            examResult.StudentScore = toUpdate.StudentScore;
        }

        return Result.Ok(examResult);
    }
}