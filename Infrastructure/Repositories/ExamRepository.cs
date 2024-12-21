using Application.Interfaces.Persistence;
using Core.Constants;
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

    public async Task<bool> HasExamResults(int examId)
    {
        return await context.ExamResults.AnyAsync(e => e.ExamId == examId);
    }

    public Task<List<Exam>> GetAllAsync()
    {
        return context.Exams.ToListAsync();
    }

    public async Task<Result<Exam>> GetByIdAsync(int id)
    {
        var exam = await context.Exams
            .FirstOrDefaultAsync(e => e.Id == id);

        if (exam == null)
            return Result.Fail(["Exam not found", ErrorType.NotFound]);

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
            return Result.Fail<List<Exam>>(["Subject not found", ErrorType.NotFound]);

        var exams = await context.Exams.Where(s => s.SubjectId == subjectId)
            .ToListAsync();
        return Result.Ok(exams);
    }

    public async Task<Result<Exam>> GetRandomExam(int subjectId)
    {
        var examId = await
            context.Exams
                .Where(exam => exam.SubjectId == subjectId)
                .Join(context.ExamQuestions, exam => exam.Id, examQuestion => examQuestion.ExamId,
                    (exam, examQuestion) => exam)
                .OrderBy(_ => Guid.NewGuid())
                .Select(exam => exam.Id)
                .FirstOrDefaultAsync();
        if (examId is 0)
            return Result.Fail(["Exam not found", ErrorType.NotFound]);

        var exam = await GetByIdAsync(examId);
        return exam.IsSuccess ? Result.Ok(exam.Value) : Result.Fail("Failed to get exam");
    }


    public async Task UpdateExamQuestions(int examId, List<int> newQuestions)
    {
        var existingQuestions = await context.ExamQuestions
            .Where(e => e.ExamId == examId)
            .Select(e => e.QuestionId)
            .ToListAsync();

        var questionToBeAdded = newQuestions.Except(existingQuestions).ToList();
        var questionToBeRemoved = existingQuestions.Except(newQuestions).ToList();

        // add the new questions;
        var addingTask = context.ExamQuestions.AddRangeAsync(questionToBeAdded.Select(id => new ExamQuestion
            { ExamId = examId, QuestionId = id }));

        // await removingTask;
        context.ExamQuestions.RemoveRange(questionToBeRemoved.Select(id => new ExamQuestion
            { ExamId = examId, QuestionId = id }));

        await addingTask;
    }

    public async Task<bool> ExamExistsForSubject(int subjectId, int examId)
    {
        return await context.Subjects.Where(s => s.Exams.Any(e => e.Id == examId)).AnyAsync(e => e.Id == subjectId);
    }
}