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
public class StudentController(IStudentServices studentServices) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = AuthRolesConstants.Admin)]
    public async Task<ActionResult<JsonResponse<List<StudentDto>>>> GetAllStudents()
    {
        var students = await studentServices.GetAllAsync();
        return Ok(JsonResponse<List<StudentDto>>.Ok(students));
    }

    [HttpGet("{studentId}")]
    public async Task<ActionResult<JsonResponse<StudentDto>>> GetByStudentId(string studentId)
    {
        var serviceResult = await studentServices.GetByIdAsync(studentId);

        return serviceResult.IsSuccess switch
        {
            true => Ok(JsonResponse<StudentDto>.Ok(serviceResult.Value)),
            false => NotFound(JsonResponse<StudentDto>.Error(serviceResult.Errors))
        };
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
}