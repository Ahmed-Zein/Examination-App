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
        var serviceResult = await examService.UpdateExamQestions(dto);
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
}