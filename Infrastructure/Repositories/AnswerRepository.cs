using Core.Entities;
using Core.Interfaces;
using FluentResults;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AnswerRepository(AppDbContext context) : IAnswerRepository
{
    public Task<bool> AnyAsync(int id)
    {
        return context.Answers.AnyAsync(e => e.Id == id);
    }

    public async Task<List<Answer>> GetAllAsync()
    {
        return await context.Answers.ToListAsync();
    }

    public async Task<Result<Answer>> GetByIdAsync(int id)
    {
        var answer = await context.Answers.FindAsync(id);

        return answer == null ? Result.Fail<Answer>("Answer not found") : Result.Ok(answer);
    }

    public void Delete(Answer entity)
    {
        context.Answers.Remove(entity);
    }

    public async Task AddManyAsync(List<Answer> answers, Question question)
    {
        List<Task> answersTask = [];
        answers.ForEach(
            answer =>
            {
                answer.Question = question;
                answersTask.Add(AddAsync(answer));
            }
        );
        await Task.WhenAll(answersTask);
    }

    public async Task AddAsync(Answer entity)
    {
        await context.Answers.AddAsync(entity);
    }

    public async Task<Result<Answer>> UpdateAsync(int id, Answer toUpdate)
    {
        var answerResult = await GetByIdAsync(id);
        if (!answerResult.IsSuccess)
            return answerResult;

        var answer = answerResult.Value;
        answer.Text = toUpdate.Text;
        answer.IsCorrect = toUpdate.IsCorrect;
        return answerResult;
    }
}