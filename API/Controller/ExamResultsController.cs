using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using Core.Constants;
using Core.Models;
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
    public async Task<ActionResult<JsonResponse<PagedData<ExamResultDto>>>> GetAll([FromQuery] PaginationQuery query,
        [FromQuery] SortingQuery sorting)
    {
        var examResults = await examResultService.GetAllExamResults(query, sorting);
        return Ok(JsonResponse<PagedData<ExamResultDto>>.Ok(examResults));
    }

    [HttpGet("{studentId}/results")]
    public async Task<ActionResult<JsonResponse<PagedData<ExamResultDto>>>> GetAllByStudentId(
        [FromQuery] PaginationQuery pagination, string studentId)
    {
        var serviceResults = await examResultService.GetAllByStudentId(studentId, pagination);

        return serviceResults switch
        {
            { IsSuccess: true } => Ok(JsonResponse<PagedData<ExamResultDto>>.Ok(serviceResults.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<PagedData<ExamResultDto>>.Error(serviceResults.Errors))
        };
    }
}