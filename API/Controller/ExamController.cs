using API.Models;
using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Migrations;
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
}