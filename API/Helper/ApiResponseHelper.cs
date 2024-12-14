using API.Models;
using Core.Constants;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Helper;

public static class ApiResponseHelper
{
    public static IActionResult HandelError(List<IError> errors)
    {
        var responseBody = JsonResponse<object>.Error(errors);
        return errors.Last().Message switch
        {
            ErrorType.Conflict => new ConflictObjectResult(responseBody),
            ErrorType.NotFound => new NotFoundObjectResult(responseBody),
            ErrorType.BadRequest => new BadRequestObjectResult(responseBody),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(responseBody),
            _ => new ObjectResult(responseBody) { StatusCode = StatusCodes.Status500InternalServerError }
        };
    }
}