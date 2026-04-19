using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalForge.Filters;
using SignalForge.Managers.Groups;
using SignalForge.Models.Dtos;

namespace SignalForge.Controllers;

[ApiController]
[Route("api/groups")]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IGroupManager _groupManager;

    public GroupController(IGroupManager groupManager)
    {
        _groupManager = groupManager;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    [ActivityLogging("GroupCreated", Description = "User created a new group")]
    public async Task<ActionResult<GroupDto>> CreateGroup([FromBody] CreateGroupDto input, CancellationToken cancellationToken)
    {
        var result = await _groupManager.CreateGroupAsync(GetUserId(), input, cancellationToken);
        return Created($"/api/groups/{result.Id}", result);
    }

    [HttpPost("direct/{otherUserId}")]
    [ActivityLogging("DirectMessageGroupAccessed")]
    public async Task<ActionResult<GroupDto>> GetOrCreateDirectMessageGroup(Guid otherUserId, CancellationToken cancellationToken)
    {
        var result = await _groupManager.GetDirectMessageGroupAsync(GetUserId(), otherUserId, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{groupId}/join")]
    [ActivityLogging("GroupJoined", Description = "User joined a group")]
    public async Task<IActionResult> JoinGroup(Guid groupId, [FromBody] JoinGroupDto input, CancellationToken cancellationToken)
    {
        var success = await _groupManager.JoinGroupAsync(GetUserId(), groupId, input, cancellationToken);
        return success ? Ok() : BadRequest();
    }

    [HttpPost("{groupId}/leave")]
    [ActivityLogging("GroupLeft", Description = "User left a group")]
    public async Task<IActionResult> LeaveGroup(Guid groupId, CancellationToken cancellationToken)
    {
        var success = await _groupManager.LeaveGroupAsync(GetUserId(), groupId, cancellationToken);
        return success ? Ok() : BadRequest();
    }

    [HttpGet("{groupId}")]
    public async Task<ActionResult<GroupDto>> GetGroupInfo(Guid groupId, CancellationToken cancellationToken)
    {
        var result = await _groupManager.GetGroupInfoAsync(groupId, cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("{groupId}/members")]
    public async Task<ActionResult<List<GroupMemberDto>>> GetGroupMembers(Guid groupId, CancellationToken cancellationToken)
    {
        var results = await _groupManager.GetGroupMembersAsync(groupId, cancellationToken);
        return Ok(results);
    }

    [HttpGet("my-groups")]
    public async Task<ActionResult<List<GroupDto>>> GetMyGroups(CancellationToken cancellationToken)
    {
        var results = await _groupManager.GetUserGroupsAsync(GetUserId(), cancellationToken);
        return Ok(results);
    }
}
