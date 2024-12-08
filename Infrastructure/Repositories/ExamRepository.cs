using Application.Interfaces.Persistence;
using Core.Entities;
using FluentResults;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ExamRepository(AppDbContext context) : IExamRepository
{
    public Task<bool> AnyAsync(int id)
    {
        return context.Answers.AnyAsync(e => e.Id == id);
    }

    public Task<List<Exam>> GetAllAsync()
    {
        return context.Exams.ToListAsync();
    }

    public async Task<Result<Exam>> GetByIdAsync(int id)
    {
        var exam = await context.Exams
            .Include(e => e.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (exam == null)
            return Result.Fail("Exam not found");

        var examQuestions = await context.ExamQuestions
            .Where(e => e.ExamId == id)
            .Include(q => q.Question)
            .ThenInclude(q => q.Answers)
            .ToListAsync();

        exam.Questions = examQuestions.Select(e => e.Question).ToList();
        return Result.Ok(exam);
    }

    public void Delete(Exam entity)
    {
        context.Exams.Remove(entity);
    }

    public async Task AddAsync(Exam entity)
    {
        await context.Exams.AddAsync(entity);
    }

    public async Task<Result<Exam>> UpdateAsync(int id, Exam toUpdate)
    {
        var examResult = await GetByIdAsync(id);
        if (!examResult.IsSuccess)
            return examResult;

        var exam = examResult.Value;
        exam.Duration = toUpdate.Duration;
        exam.ModelName = toUpdate.ModelName;

        return Result.Ok(exam);
    }

    public async Task<Result<List<Exam>>> GetAllBySubject(int subjectId)
    {
        if (!await context.Subjects.AnyAsync(s => s.Id == subjectId))
            return Result.Fail<List<Exam>>("Subject not found");

        var exams = await context.Exams.Where(s => s.SubjectId == subjectId)
            .ToListAsync();
        return Result.Ok(exams);
    }

    public async Task<List<int>> GetAllExamIds(int subjectId)
    {
        var exams = await context.Subjects.Where(s => s.Id == subjectId)
            .Select(e => e.Exams.Select(s => s.Id).ToList())
            .FirstOrDefaultAsync();
        return exams ?? [];
    }

    public async Task AddQuestionsToExam(int examId, List<int> questionIds)
    {
        await context.ExamQuestions.AddRangeAsync(questionIds.Select(id => new ExamQuestion
            { ExamId = examId, QuestionId = id }));
    }

    public async Task<bool> ExamExistsForSubject(int subjectId, int examId)
    {
        return await context.Subjects.Where(s => s.Exams.Any(e => e.Id == examId)).AnyAsync(e => e.Id == subjectId);
    }
}