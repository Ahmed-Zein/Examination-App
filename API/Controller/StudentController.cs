using API.Helper;
using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using Core.Constants;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Authorize]
[ApiController]
[Route("api/students")]
public class StudentController(
    ICacheStore cacheStore,
    IStudentServices studentServices,
    IDashboardService dashboardService,
    ILogger<StudentController> logger) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = AuthRolesConstants.Admin)]
    public async Task<ActionResult<JsonResponse<PagedData<StudentDto>>>> GetAllStudents(
        [FromQuery] PaginationQuery pagination)
    {
        var students = await studentServices.GetAllAsync(pagination);
        return Ok(JsonResponse<PagedData<StudentDto>>.Ok(students));
    }

    [HttpGet("{studentId}")]
    public async Task<ActionResult<JsonResponse<StudentDto>>> GetByStudentId(string studentId)
    {
        var cachingKey = new StudentKey(studentId);
        var cacheResult = cacheStore.Get(cachingKey);
        if (cacheResult.IsSuccess)
        {
            logger.LogInformation("Cache hit for studentId: {studentId} at {timeStamp}", studentId, DateTime.UtcNow);
            return Ok(JsonResponse<StudentDto>.Ok(cacheResult.Value));
        }

        var serviceResult = await studentServices.GetByIdAsync(studentId);
        if (!serviceResult.IsSuccess)
        {
            logger.LogWarning("Service failed for studentId: {studentId} at {timeStamp}", studentId, DateTime.UtcNow);
            return (ActionResult)ApiResponseHelper.HandelError(serviceResult.Errors);
        }

        cacheStore.Add(cachingKey, serviceResult.Value);
        logger.LogInformation("Cache updated for studentId: {studentId} at {timeStamp}", studentId, DateTime.UtcNow);
        return Ok(JsonResponse<StudentDto>.Ok(serviceResult.Value));
    }

    [HttpPut("lock/{studentId}")]
    public async Task<ActionResult<JsonResponse<StudentDto>>> ToggleStudentLock(string studentId)
    {
        var serviceResult = await studentServices.ToggleStudentLock(studentId);
        return serviceResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<StudentDto>.Ok(serviceResult.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<StudentDto>.Error(serviceResult.Errors))
        };
    }

    [HttpGet("{studentId}/dashboard")]
    public async Task<ActionResult<JsonResponse<StudentDashboard>>> Dashboard(string studentId)
    {
        var serviceResult = await dashboardService.GetStudentDashboard(studentId);
        return serviceResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<StudentDashboard>.Ok(serviceResult.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<StudentDashboard>.Error(serviceResult.Errors))
        };
    }
}