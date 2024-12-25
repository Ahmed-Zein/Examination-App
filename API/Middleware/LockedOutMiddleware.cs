using System.Net;
using System.Security.Claims;
using Application.Interfaces;
using Application.Models;
using Core.Constants;

namespace API.Middleware;

public class LockedOutMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        if (CheckRoleClaim(context))
        {
            await next(context);
            return;
        }

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var serviceResult = await authService.IsUserLockedOut(userId!);
        if (!serviceResult.IsSuccess)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            var jsonResponse = JsonResponse<string>.Error(serviceResult.Errors);
            await context.Response.WriteAsJsonAsync(jsonResponse);
            return;
        }

        await next(context);
    }

    /// <param name="context"></param>
    /// <returns>true if an Admin role is found</returns>
    /// <returns>true if the token is missing letting the controller handel this case</returns>
    /// <returns>false if a Student role is found</returns>
    private static bool CheckRoleClaim(HttpContext context)
    {
        var roleClaim = context.User.FindFirstValue(ClaimTypes.Role);
        var isRoleEmpty = string.IsNullOrEmpty(roleClaim);
        var isAdminRole = roleClaim == AuthRolesConstants.Admin;
        return isRoleEmpty || isAdminRole;
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