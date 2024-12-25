using Application.Interfaces;
using Core.Constants;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = AuthRolesConstants.Admin)]
public class AdminController(IDashboardService dashboardService) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<AdminDashboard>> Dashboard()
    {
        return Ok(await dashboardService.GetAdminDashboard());
    }
}