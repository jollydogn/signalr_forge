using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalForge.Managers.Presence;
using SignalForge.Models.Dtos;

namespace SignalForge.Controllers;

[ApiController]
[Route("api/presence")]
[Authorize]
public class PresenceController : ControllerBase
{
    private readonly IPresenceManager _presenceManager;

    public PresenceController(IPresenceManager presenceManager)
    {
        _presenceManager = presenceManager;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserPresenceDto>> GetUserPresence(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _presenceManager.GetUserPresenceAsync(userId, cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("online")]
    public async Task<ActionResult<List<UserPresenceDto>>> GetOnlineUsers(CancellationToken cancellationToken)
    {
        var results = await _presenceManager.GetOnlineUsersAsync(cancellationToken);
        return Ok(results);
    }

    [HttpGet("groups/{groupId}")]
    public async Task<ActionResult<List<UserPresenceDto>>> GetGroupPresence(Guid groupId, CancellationToken cancellationToken)
    {
        var results = await _presenceManager.GetGroupPresenceAsync(groupId, cancellationToken);
        return Ok(results);
    }
}
