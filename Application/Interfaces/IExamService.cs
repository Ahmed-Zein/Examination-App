using Application.DTOs;
using FluentResults;

namespace Application.Interfaces;

public interface IExamService
{
    Task<Result<ExamDto>> CreateExam(CreateExamDto createExamDto, int subjectId);
    Task<Result<ExamDto>> GetExamById(int id);
    Task<Result<List<ExamDto>>> GetExams(int subjectId);
    Task<Result<ExamDto>> GetRandomExam(int subjectId);
    Task<Result> AddQuestionToExam(AddQuestionToExamDto questionDto);
}