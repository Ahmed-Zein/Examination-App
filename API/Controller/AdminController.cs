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
    public async Task<ActionResult> GetAllStudents()
    {
        return Ok(await studentServices.GetAllAsync());
    }

    [HttpGet("/students/{studentId}")]
    public async Task<ActionResult> GetByStudentId(string studentId)
    {
        var studentResult = await studentServices.GetByIdAsync(studentId);

        return studentResult.IsSuccess switch
        {
            true => Ok(studentResult.Value),
            false => BadRequest(studentResult.Errors)
        };
    }
}