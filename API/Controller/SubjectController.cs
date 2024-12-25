using API.Helper;
using Application.DTOs;
using Application.Interfaces;
using Application.Models;
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
        var serviceResult = await subjectService.GetById(subjectId);
        return serviceResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<SubjectDto>.Ok(serviceResult.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<SubjectDto>.Error(serviceResult.Errors))
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
        var serviceResult = await subjectService.UpdateSubject(updateSubjectDto, subjectId);
        return serviceResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<SubjectDto>.Ok(serviceResult.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<SubjectDto>.Error(serviceResult.Errors))
        };
    }

    [HttpDelete("{subjectId:int:min(1)}")]
    [Authorize(Roles = AuthRolesConstants.Admin)]
    public async Task<ActionResult> DeleteSubject(int subjectId)
    {
        var serviceResult = await subjectService.DeleteSubject(subjectId);
        if (serviceResult.IsSuccess)
            return NoContent();

        return (ActionResult)ApiResponseHelper.HandelError(serviceResult.Errors);
    }
}