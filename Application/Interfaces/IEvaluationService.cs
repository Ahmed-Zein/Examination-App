using Application.DTOs;
using FluentResults;

namespace Application.Interfaces;

public interface IEvaluationService
{
    Task<Result> ReceiveExamSolution(string studentId, int examId, ExamSolutionsDto examSolutionsDto);
    Task<Result> EvaluateExam(int examId, ExamSolutionsDto examSolutionsDto);
}