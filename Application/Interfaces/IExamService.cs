using Application.DTOs;
using FluentResults;

namespace Application.Interfaces;

public interface IExamService
{
    Task<Result<ExamDto>> CreateExam(CreateExamDto createExamDto, int subjectId);
    Task<Result<ExamDto>> GetExamById(int id);
    Task<Result<List<ExamDto>>> GetExams(int subjectId);
    Task<Result<StudentExam>> PullExam(string userId, int subjectId);
    Task<Result> UpdateExamQuestions(AddQuestionToExamDto questionDto);
    Task<Result> DeleteExam(int examId);
    Task<Result<ExamDto>> UpdateExam(int examId, UpdateExamDto examDto);
}