using Application.DTOs;
using Application.Interfaces;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Authorize]
[ApiController]
[Route("api/subjects")]
public class SubjectController(ISubjectService subjectService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SubjectDto>>> Get()
    {
        return Ok(await subjectService.GetAllSubjects());
    }

    [HttpGet("{subjectId:int:min(1)}")]
    public async Task<ActionResult<SubjectDto>> GetById(int subjectId)
    {
        var subjectResult = await subjectService.GetById(subjectId);
        return subjectResult switch
        {
            { IsSuccess: true } => Ok(subjectResult.Value),
            { IsSuccess: false } => NotFound(subjectResult.Errors)
        };
    }

    [HttpPost]
    [Authorize(Roles = AuthRolesConstants.Admin)]
    public async Task<ActionResult<SubjectDto>> AddSubject([FromBody] CreateSubjectDto createSubjectDto)
    {
        return Ok(await subjectService.CreateSubject(createSubjectDto));
    }

    [HttpPut("{subjectId:int:min(1)}")]
    [Authorize(Roles = AuthRolesConstants.Admin)]
    public async Task<ActionResult<SubjectDto>> AddSubject([FromBody] UpdateSubjectDto updateSubjectDto, int subjectId)
    {
        var updateSubjectResult = await subjectService.UpdateSubject(updateSubjectDto, subjectId);
        return updateSubjectResult switch
        {
            { IsSuccess: true } => Ok(updateSubjectResult.Value),
            { IsSuccess: false } => NotFound(updateSubjectResult.Errors)
        };
    }
}