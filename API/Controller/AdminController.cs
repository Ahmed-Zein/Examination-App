using API.Models;
using Application.DTOs;
using Application.Interfaces;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = AuthRolesConstants.Admin)]
public class AdminController(IStudentServices studentServices) : ControllerBase
{
    [HttpGet("/students")]
    public async Task<ActionResult<JsonResponse<List<StudentDto>>>> GetAllStudents()
    {
        var students = await studentServices.GetAllAsync();
        return Ok(JsonResponse<List<StudentDto>>.Ok(students));
    }

    [HttpGet("/students/{studentId}")]
    public async Task<ActionResult<JsonResponse<StudentDto>>> GetByStudentId(string studentId)
    {
        var studentResult = await studentServices.GetByIdAsync(studentId);

        return studentResult.IsSuccess switch
        {
            true => Ok(JsonResponse<StudentDto>.Ok(studentResult.Value)),
            false => BadRequest(JsonResponse<StudentDto>.Error(studentResult.Errors))
        };
    }
}