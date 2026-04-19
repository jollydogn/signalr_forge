using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalForge.Managers.Logging;
using SignalForge.Models.Dtos;

namespace SignalForge.Controllers;

[ApiController]
[Route("api/logs")]
[Authorize(Roles = "admin")] // Example restricted access
public class LogsController : ControllerBase
{
    private readonly ILogManager _logManager;

    public LogsController(ILogManager logManager)
    {
        _logManager = logManager;
    }

    [HttpGet("requests")]
    public async Task<ActionResult<List<RequestLogDto>>> GetRecentRequests([FromQuery] int count = 100, CancellationToken cancellationToken = default)
    {
        var results = await _logManager.GetRecentRequestsAsync(count, cancellationToken);
        return Ok(results);
    }

    [HttpGet("activities/{userId}")]
    public async Task<ActionResult<List<ActivityLogDto>>> GetUserActivities(string userId, [FromQuery] int count = 50, CancellationToken cancellationToken = default)
    {
        var results = await _logManager.GetUserActivitiesAsync(userId, count, cancellationToken);
        return Ok(results);
    }
}
