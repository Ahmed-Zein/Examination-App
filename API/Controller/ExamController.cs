using System.Security.Claims;
using API.Models;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Authorize]
[ApiController]
[Route("api/subjects/{subjectId:int:min(0)}/exams")]
public class ExamController(IExamService examService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<JsonResponse<List<ExamDto>>>> GetAllExams(int subjectId)
    {
        var serviceResult = await examService.GetExams(subjectId);
        return serviceResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<List<ExamDto>>.Ok(serviceResult.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<List<ExamDto>>.Error(serviceResult.Errors))
        };
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<JsonResponse<ExamDto>>> CreateExam([FromBody] CreateExamDto examDto, int subjectId)
    {
        var serviceResult = await examService.CreateExam(examDto, subjectId);
        return serviceResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<ExamDto>.Ok(serviceResult.Value)),
            { IsSuccess: false } => BadRequest(JsonResponse<ExamDto>.Error(serviceResult.Errors))
        };
    }

    [HttpPut("{examId:int:min(1)}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<JsonResponse<object>>> UpdateExamQuestions([FromBody] List<int> questionIds,
        int subjectId, int examId)
    {
        var dto = new AddQuestionToExamDto { QuestionIds = questionIds, SubjectId = subjectId, ExamId = examId };
        var serviceResult = await examService.UpdateExamQuestions(dto);
        return serviceResult switch
        {
            { IsSuccess: true } => CreatedAtAction(nameof(GetExamById), new { dto.ExamId, subjectId },
                JsonResponse<string>.Ok("", "Questions updated Successfully.")),
            { IsSuccess: false } => BadRequest(JsonResponse<object>.Error(serviceResult.Errors))
        };
    }

    [HttpGet("{examId:int:min(0)}")]
    public async Task<ActionResult<JsonResponse<ExamDto>>> GetExamById(int subjectId, int examId)
    {
        var serviceResult = await examService.GetExamById(examId);
        return serviceResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<ExamDto>.Ok(serviceResult.Value)),
            { IsSuccess: false } => BadRequest(JsonResponse<ExamDto>.Error(serviceResult.Errors))
        };
    }

    [HttpGet("start")]
    public async Task<ActionResult<JsonResponse<StudentExam>>> StartStudentExam(int subjectId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return Unauthorized(JsonResponse<StudentExam>.Error(["Invalid user Token"]));

        var serviceResult = await examService.GetRandomExam(userId, subjectId);
        return serviceResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<StudentExam>.Ok(serviceResult.Value)),
            { IsSuccess: false } => BadRequest(JsonResponse<StudentExam>.Error(serviceResult.Errors))
        };
    }

    [HttpPost("{examId:int:min(1)}/evaluation")]
    public async Task<ActionResult<JsonResponse<StudentExam>>> EvaluateStudentExam(int examId, int subjectId,
        [FromBody] ExamSolutionsDto examSolutionsDto)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return Unauthorized(JsonResponse<StudentExam>.Error(["Invalid user Token"]));

        var serviceResult = await examService.EvaluateExam(userId, examId, examSolutionsDto);
        return serviceResult switch
        {
            { IsSuccess: true } => Ok(),
            { IsSuccess: false } => BadRequest(JsonResponse<StudentExam>.Error(serviceResult.Errors))
        };
    }

    [HttpDelete("{examId:int:min(1)}")]
    public async Task<ActionResult<JsonResponse<StudentExam>>> DeleteExam(int examId, int subjectId)
    {
        var serviceResult = await examService.DeleteExam(examId);
        return serviceResult switch
        {
            { IsSuccess: true } => NoContent(),
            { IsSuccess: false } => BadRequest(JsonResponse<StudentExam>.Error(serviceResult.Errors))
        };
    }
}