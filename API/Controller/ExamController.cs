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
    public async Task<ActionResult<JsonResponse<List<ExamDto>>>> Get(int subjectId)
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
    public async Task<ActionResult<JsonResponse<ExamDto>>> AddExam([FromBody] CreateExamDto examDto, int subjectId)
    {
        var serviceResult = await examService.CreateExam(examDto, subjectId);
        return serviceResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<ExamDto>.Ok(serviceResult.Value)),
            { IsSuccess: false } => BadRequest(JsonResponse<ExamDto>.Error(serviceResult.Errors))
        };
    }

    [HttpPost("{examId:int:min(1)}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<JsonResponse<object>>> AddQuestion([FromBody] List<int> questionIds,
        int subjectId, int examId)
    {
        var dto = new AddQuestionToExamDto { QuestionIds = questionIds, SubjectId = subjectId, ExamId = examId };
        var serviceResult = await examService.AddQuestionToExam(dto);
        return serviceResult switch
        {
            { IsSuccess: true } => CreatedAtAction(nameof(GetExam), new { dto.ExamId, subjectId },
                JsonResponse<string>.Ok("", "Questions Added Successfully")),
            { IsSuccess: false } => BadRequest(JsonResponse<object>.Error(serviceResult.Errors))
        };
    }

    [HttpGet("{examId:int:min(0)}")]
    public async Task<ActionResult<JsonResponse<ExamDto>>> GetExam(int subjectId, int examId)
    {
        var serviceResult = await examService.GetExamById(examId);
        return serviceResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<ExamDto>.Ok(serviceResult.Value)),
            { IsSuccess: false } => BadRequest(JsonResponse<ExamDto>.Error(serviceResult.Errors))
        };
    }
}