using System.Linq.Expressions;
using Core.Constants;
using Core.Entities;
using Core.Models;
using Core.Persistence;
using FluentResults;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ExamResultRepository(AppDbContext context, IPaginationDataBuilder<ExamResult> pageBuilder)
    : IExamResultRepository
{
    public Task<bool> AnyAsync(int id)
    {
        return context.ExamResults.AnyAsync(e => e.Id == id);
    }

    public async Task<PagedData<ExamResult>> GetAllAsync(PaginationQuery pagination, SortingQuery sortingQuery)
    {
        var query = context.ExamResults
            .Include(e => e.AppUser)
            .AsNoTracking();
        query = sortingQuery.Ascending ?? true
            ? query.OrderBy(GetSortingProperty(sortingQuery))
            : query.OrderByDescending(GetSortingProperty(sortingQuery));

        return await pageBuilder
            .WithQueryable(query)
            .WithPagination(pagination)
            .Build();
    }


    public async Task<List<ExamResult>> GetAllAsync()
    {
        return await context.ExamResults
            .Include(e => e.AppUser)
            .ToListAsync();
    }

    public async Task<Result<PagedData<ExamResult>>> GetByStudentId(string studentId, PaginationQuery pagination)
    {
        var query = context.ExamResults
            .Where(e => e.AppUserId == studentId)
            .OrderByDescending(e => e.StartTime)
            .AsNoTracking();

        return Result.Ok(await pageBuilder
            .WithQueryable(query)
            .WithPagination(pagination)
            .Build());
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

    private static Expression<Func<ExamResult, object>> GetSortingProperty(SortingQuery sortingQuery)
    {
        return sortingQuery.OrderBy?.ToLower() switch
        {
            "endtime" => examResult => examResult.EndTime ?? new DateTime(),
            "studentscore" => examResult => examResult.StudentScore,
            "status" => examResult => examResult.Status,
            _ => examResult => examResult.StartTime
        };
    }
}