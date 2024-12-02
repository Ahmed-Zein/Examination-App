using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[ApiController]
[Route("api/admin")]
public class AdminController(AppDbContext context) : ControllerBase
{
    [HttpGet("/students")]
    public IActionResult GetAllStudents()
    {
        return Ok();
    }
}