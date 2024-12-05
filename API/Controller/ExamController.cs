using API.Models;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Route("api/subjects/{subjectId:int:min(0)}/exams")]
[ApiController]
public class ExamController(IExamService examService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<JsonResponse<List<ExamDto>>>> Get(int subjectId)
    {
        var examResult = await examService.GetExams(subjectId);

        return examResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<List<ExamDto>>.Ok(examResult.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<List<ExamDto>>.Error(examResult.Errors))
        };
    }

    [HttpPost]
    public async Task<ActionResult<JsonResponse<ExamDto>>> AddExam([FromBody] CreateExamDto examDto, int subjectId)
    {
        var createdExamResult = await examService.CreateExam(examDto, subjectId);
        return createdExamResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<ExamDto>.Ok(createdExamResult.Value)),
            { IsSuccess: false } => BadRequest(JsonResponse<ExamDto>.Error(createdExamResult.Errors))
        };
    }

    [HttpPost("add")]
    public async Task<ActionResult<JsonResponse<object>>> AddQuestion([FromBody] AddQuestionToExamDto questionIds,
        int subjectId)
    {
        var addQuestionsResult = await examService.AddQuestionToExam(questionIds);
        return addQuestionsResult switch
        {
            { IsSuccess: true } => CreatedAtAction(nameof(GetExam), new { questionIds.ExamId, subjectId },
                JsonResponse<string>.Ok("", "Questions Added Successfully")),
            { IsSuccess: false } => BadRequest(JsonResponse<object>.Error(addQuestionsResult.Errors))
        };
    }

    [HttpGet("{examId:int:min(0)}")]
    public async Task<ActionResult<JsonResponse<ExamDto>>> GetExam(int subjectId, int examId)
    {
        return Ok();
    }
}