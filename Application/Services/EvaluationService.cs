using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using Core.Constants;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Persistence;
using FluentResults;

namespace Application.Services;

public class EvaluationService(IUnitOfWork unitOfWork, IRabbitPublisher rabbitPublisher) : IEvaluationService
{
    public async Task<Result> ReceiveExamSolution(string studentId, int examId, ExamSolutionsDto examSolutionsDto)
    {
        var validationResult = await IsValidExamSolution(examId, examSolutionsDto);
        if (!validationResult.IsSuccess)
            return validationResult.ToResult();

        var examResult = validationResult.Value;
        examResult.Status = ExamResultStatus.InEvaluation;
        examResult.EndTime = DateTime.UtcNow;

        await unitOfWork.CommitAsync();

        await rabbitPublisher.Publish(new RabbitExamRequest
            { StudentId = studentId, ExamId = examId, Solutions = examSolutionsDto });
        return Result.Ok();
    }

    public async Task<Result> EvaluateExam(int examId, ExamSolutionsDto examSolutionsDto)
    {
        var validationResult = await IsValidExamSolution(examId, examSolutionsDto);
        if (!validationResult.IsSuccess)
            return validationResult.ToResult();

        var examResult = validationResult.Value;
        var examQuestions = await unitOfWork.QuestionRepository.GetByExamId(examId);
        var studentScore = 0;
        foreach (var solution in examSolutionsDto.Solutions)
        {
            var question = examQuestions.Find(q => q.Id == solution.QuestionId);
            if (question is null)
                return Result.Fail(["The question does not exists", ErrorType.BadRequest]);

            var answer = question.Answers.Find(an => an.Id == solution.AnswerId);
            if (answer is null)
                return Result.Fail(["The question does not have a valid answer", ErrorType.BadRequest]);
            if (answer.IsCorrect)
                studentScore += 1;
        }

        var endTime = examResult.EndTime ?? DateTime.UtcNow;

        if (examResult.Exam is not null
            && examResult.StartTime + examResult.Exam.Duration >= endTime)
            examResult.StudentScore = studentScore;

        examResult.Status = ExamResultStatus.Evaluated;
        examResult.EndTime = endTime;
        await unitOfWork.CommitAsync();
        return Result.Ok();
    }


    public async Task<Result<ExamResult>> IsValidExamSolution(int examId, ExamSolutionsDto examSolutionsDto)
    {
        // TODO: Move this to its own Fluent Validator Class??
        var repositoryResult = await unitOfWork.ExamResultRepository.GetByIdAsync(examSolutionsDto.ExamResultId);

        if (!repositoryResult.IsSuccess)
            return repositoryResult.ToResult();

        var examResult = repositoryResult.Value;

        return examResult.Status is ExamResultStatus.Evaluated
            ? Result.Fail(["The exam is already evaluated", ErrorType.Conflict])
            : Result.Ok(examResult);
    }
}