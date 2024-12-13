using API.Models;
using Application.DTOs;
using Application.Interfaces;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Authorize]
[ApiController]
[Route("api/students")]
public class ExamResultsController(IExamResultService examResultService) : ControllerBase
{
    [HttpGet("results")]
    [Authorize(Roles = AuthRolesConstants.Admin)]
    public async Task<ActionResult<JsonResponse<List<ExamResultDto>>>> GetAll()
    {
        var examResults = await examResultService.GetAllExamResults();
        return Ok(JsonResponse<List<ExamResultDto>>.Ok(examResults));
    }

    [HttpGet("{studentId}/results")]
    public async Task<ActionResult<JsonResponse<List<ExamResultDto>>>> GetAllByStudentId(string studentId)
    {
        var serviceResults = await examResultService.GetAllByStudentId(studentId);

        return serviceResults switch
        {
            { IsSuccess: true } => Ok(JsonResponse<List<ExamResultDto>>.Ok(serviceResults.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<List<ExamResultDto>>.Error(serviceResults.Errors))
        };
    }
}