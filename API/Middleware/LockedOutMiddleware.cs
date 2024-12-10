using System.Net;
using System.Security.Claims;
using API.Models;
using Application.Interfaces;
using Core.Constants;

namespace API.Middleware;

public class LockedOutMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        var role = context.User.FindFirstValue(ClaimTypes.Role)!;
        if (role == AuthRolesConstants.Admin || !context.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
        {
            await next(context);
            return;
        }

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var serviceResult = await authService.IsUserLockedOut(userId!);
        if (!serviceResult.IsSuccess)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            var js = JsonResponse<string>.Error(serviceResult.Errors);
            await context.Response.WriteAsJsonAsync(js);
            return;
        }

        if (serviceResult.Value)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsJsonAsync(JsonResponse<string>.Error(serviceResult.Errors));
            return;
        }

        await next(context);
    }
}

public static class LockoutMiddlewareExtension
{
    public static IApplicationBuilder UseLockedOutMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LockedOutMiddleware>();
    }
}