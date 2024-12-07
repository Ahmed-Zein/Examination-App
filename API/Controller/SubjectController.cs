using API.Models;
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
    public async Task<ActionResult<JsonResponse<List<SubjectDto>>>> Get()
    {
        var subjects = await subjectService.GetAllSubjects();

        return Ok(JsonResponse<List<SubjectDto>>.Ok(subjects));
    }


    [HttpGet("{subjectId:int:min(1)}")]
    public async Task<ActionResult<JsonResponse<SubjectDto>>> GetById(int subjectId)
    {
        var subjectResult = await subjectService.GetById(subjectId);
        return subjectResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<SubjectDto>.Ok(subjectResult.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<SubjectDto>.Error(subjectResult.Errors))
        };
    }

    [HttpPost]
    [Authorize(Roles = AuthRolesConstants.Admin)]
    public async Task<ActionResult<JsonResponse<SubjectDto>>> AddSubject(
        [FromBody] List<CreateSubjectDto> createSubjectDto)
    {
        var createdSubject = await subjectService.CreateSubject(createSubjectDto);
        return Ok(JsonResponse<List<SubjectDto>>.Ok(createdSubject));
    }

    [HttpPut("{subjectId:int:min(1)}")]
    [Authorize(Roles = AuthRolesConstants.Admin)]
    public async Task<ActionResult<JsonResponse<SubjectDto>>> AddSubject([FromBody] UpdateSubjectDto updateSubjectDto,
        int subjectId)
    {
        var updateSubjectResult = await subjectService.UpdateSubject(updateSubjectDto, subjectId);
        return updateSubjectResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<SubjectDto>.Ok(updateSubjectResult.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<SubjectDto>.Error(updateSubjectResult.Errors))
        };
    }
}