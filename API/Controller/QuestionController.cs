using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[ApiController]
[Authorize(Roles = AuthRolesConstants.Admin)]
[Route("api/subjects/{subjectId:int:min(1)}/questions")]
public class QuestionController(IQuestionService questionService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<JsonResponse<List<QuestionDto>>>> GetQuestions(int subjectId)
    {
        var serviceResult = await questionService.GetBySubject(subjectId);
        return Ok(JsonResponse<List<QuestionDto>>.Ok(serviceResult));
    }

    [HttpGet("{questionId:int:min(1)}")]
    public async Task<ActionResult<JsonResponse<QuestionDto>>> GetById(int questionId, int subjectId)
    {
        var queryResult = await questionService.GetQuestion(questionId);
        return queryResult switch
        {
            { IsSuccess: true } => Ok(JsonResponse<QuestionDto>.Ok(queryResult.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<QuestionDto>.Ok(queryResult.Value))
        };
    }

    [HttpPost]
    public async Task<ActionResult<JsonResponse<List<QuestionDto>>>> CreateQuestion(
        [FromBody] List<CreateQuestionDto> createQuestionDto,
        int subjectId)
    {
        var createResult = await questionService.AddQuestion(createQuestionDto, subjectId);
        return createResult switch
        {
            { IsSuccess: true } => CreatedAtAction(nameof(GetQuestions),
                new { subjectId },
                JsonResponse<List<QuestionDto>>.Ok(createResult.Value)),
            { IsSuccess: false } => BadRequest(JsonResponse<List<QuestionDto>>.Error(createResult.Errors))
        };
    }

    [HttpDelete("{questionId:int:min(1)}")]
    public async Task<ActionResult> DeleteQuestion(int subjectId, int questionId)
    {
        var serviceResult = await questionService.DeleteQuestion(questionId);
        return serviceResult switch
        {
            { IsSuccess: true } => NoContent(),
            { IsSuccess: false } => BadRequest(JsonResponse<string>.Error(serviceResult.Errors))
        };
    }
}