using Core.Entities;
using Core.Interfaces.Repositories;
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
        var question = await context.Questions.FindAsync(id);
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
}