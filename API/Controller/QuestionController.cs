using API.Models;
using Application.DTOs;
using Application.Interfaces;
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
            { IsSuccess: false } => NotFound(JsonResponse<QuestionDto>.Ok(queryResult.Value)),
        };
    }

    [HttpPost]
    public async Task<ActionResult<JsonResponse<QuestionDto>>> CreateQuestion(CreateQuestionDto createQuestionDto,
        int subjectId)
    {
        var createResult = await questionService.AddQuestion(createQuestionDto, subjectId);
        return createResult switch
        {
            { IsSuccess: true } => CreatedAtAction(nameof(GetById),
                new { questionId = createResult.Value.Id, subjectId },
                JsonResponse<QuestionDto>.Ok(createResult.Value)),
            { IsSuccess: false } => NotFound(JsonResponse<QuestionDto>.Error(createResult.Errors)),
        };
    }
}