using Application.Interfaces.Persistence;
using Core.Entities;
using FluentResults;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuestionRepository(AppDbContext context) : IQuestionRepository
{
    public async Task<bool> AnyAsync(int id)
    {
        return await context.Questions.AnyAsync(e => e.Id == id);
    }

    public async Task<List<Question>> GetAllAsync()
    {
        return await context.Questions.ToListAsync();
    }

    public async Task<Result<Question>> GetByIdAsync(int id)
    {
        var query = context.Questions.Include(e => e.Answers);
        var question = await query.FirstOrDefaultAsync(e => e.Id == id);
        return question switch
        {
            null => Result.Fail<Question>("Question not found"),
            _ => Result.Ok(question)
        };
    }

    public void Delete(Question entity)
    {
        context.Questions.Remove(entity);
    }

    public async Task AddAsync(Question entity)
    {
        await context.Questions.AddAsync(entity);
    }

    public async Task<Result<Question>> UpdateAsync(int id, Question toUpdate)
    {
        var questionResult = await GetByIdAsync(id);
        if (questionResult.IsFailed)
            return questionResult;

        var question = questionResult.Value;
        question.Text = toUpdate.Text;

        return questionResult;
    }

    public async Task<List<int>> GetQuestionsIdBySubject(int subjectId)
    {
        return await context.Questions.Where(q => q.SubjectId == subjectId).Select(q => q.Id).ToListAsync();
    }

    public async Task<List<int>> GetIdByExam(int examId)
    {
        return await context.ExamQuestions.Where(e => e.ExamId == examId)
            .Select(question => question.QuestionId).ToListAsync();
    }

    public Task<List<Question>> GetBySubjectId(int subjectId)
    {
        var query = context.Questions.Include(q => q.Answers).Where(q => q.SubjectId == subjectId);
        return query.ToListAsync();
    }
}