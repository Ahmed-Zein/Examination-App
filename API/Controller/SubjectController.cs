using Core.Constants;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Authorize]
[ApiController]
[Route("api/subjects")]
public class SubjectController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }

    [Authorize(Roles = AuthRolesConstants.Student)] // TODO: Figure out if it is OK to reference the core from the API layer
    [HttpPost("{subjectId:int:min(1)}")]
    public IActionResult Update(string subjectId)
    {
        return Ok();
    }

    [HttpPut]
    public IActionResult Add()
    {
        return Ok();
    }
}