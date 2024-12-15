using Application.Interfaces.Persistence;
using Application.Models;
using Core.Constants;
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

    public Task<PagedData<ExamResult>> GetAllAsync(PaginationQuery query)
    {
        var examResultsQuery = context.ExamResults
            .Include(e => e.AppUser)
            .OrderBy(e => e.StartTime)
            .AsNoTracking();

        return PagedData<ExamResult>.CreateAsync(examResultsQuery, query);
    }

    public async Task<List<ExamResult>> GetAllAsync()
    {
        return await context.ExamResults
            .Include(e => e.AppUser)
            .ToListAsync();
    }

    public async Task<Result<PagedData<ExamResult>>> GetByStudentId(string studentId, PaginationQuery pagination)
    {
        var examQuery = context.ExamResults
            .Where(e => e.AppUserId == studentId)
            .OrderBy(e => e.StartTime)
            .AsNoTracking();

        return Result.Ok(await PagedData<ExamResult>.CreateAsync(examQuery, pagination));
    }

    public async Task<Result<ExamResult>> GetByIdAsync(int id)
    {
        var exam = await context.ExamResults
            .Where(e => e.Id == id)
            .Include(e => e.Exam)
            .FirstOrDefaultAsync();

        return exam is not null
            ? Result.Ok(exam)
            : Result.Fail(["Exam Result not found", ErrorType.NotFound]);
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
            return Result.Fail(["exam result not found", ErrorType.NotFound]);

        var examDurationTask = context.Exams.Where(e => e.Id == examResult.ExamId)
            .Select(e => e.Duration)
            .FirstOrDefaultAsync();

        examResult.Status = toUpdate.Status;
        examResult.EndTime = toUpdate.EndTime;

        var examDuration = await examDurationTask;
        if (examResult.StartTime + examDuration < toUpdate.EndTime) examResult.StudentScore = toUpdate.StudentScore;

        return Result.Ok(examResult);
    }
}