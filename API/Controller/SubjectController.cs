using API.Helper;
using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using Core.Constants;
using Core.Interfaces;
using Infrastructure.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[Authorize]
[ApiController]
[Route("api/subjects")]
public class SubjectController(
    ICacheStore cacheStore,
    ISubjectService subjectService,
    ILogger<SubjectController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<JsonResponse<List<SubjectDto>>>> Get()
    {
        var cachingKey = new SubjectsListKey();
        var cacheResults = cacheStore.Get(cachingKey);
        if (cacheResults.IsSuccess)
        {
            logger.LogInformation("Cache hit for {noOfSubjects} students at {timeStamp}.", cacheResults.Value.Count,
                DateTime.Now);
            return Ok(JsonResponse<List<SubjectDto>>.Ok(cacheResults.Value));
        }

        var subjects = await subjectService.GetAllSubjects();
        cacheStore.Add(cachingKey, subjects);

        logger.LogInformation("Cache updated for {noOfSubjects} students at {timeStamp}.", cacheResults.Value.Count,
            DateTime.Now);
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