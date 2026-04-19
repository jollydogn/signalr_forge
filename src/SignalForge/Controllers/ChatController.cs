using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalForge.Filters;
using SignalForge.Managers.Chat;
using SignalForge.Models.Dtos;

namespace SignalForge.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatManager _chatManager;

    public ChatController(IChatManager chatManager)
    {
        _chatManager = chatManager;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost("messages")]
    [ActivityLogging("MessageSent", Description = "User sent a chat message via REST API")]
    public async Task<ActionResult<MessageDto>> SendMessage([FromBody] SendMessageDto input, CancellationToken cancellationToken)
    {
        var result = await _chatManager.SendMessageAsync(GetUserId(), input, cancellationToken);
        return Ok(result);
    }

    [HttpPut("messages/{messageId}")]
    [ActivityLogging("MessageEdited", Description = "User edited a chat message")]
    public async Task<ActionResult<MessageDto>> EditMessage(Guid messageId, [FromBody] string newContent, CancellationToken cancellationToken)
    {
        var result = await _chatManager.EditMessageAsync(GetUserId(), messageId, newContent, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("messages/{messageId}")]
    [ActivityLogging("MessageDeleted", Description = "User deleted a chat message")]
    public async Task<IActionResult> DeleteMessage(Guid messageId, CancellationToken cancellationToken)
    {
        var success = await _chatManager.DeleteMessageAsync(GetUserId(), messageId, cancellationToken);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("messages/{messageId}/read")]
    [ActivityLogging("MessageRead", Description = "User marked a message as read")]
    public async Task<IActionResult> MarkAsRead(Guid messageId, CancellationToken cancellationToken)
    {
        var success = await _chatManager.MarkAsReadAsync(GetUserId(), messageId, cancellationToken);
        return success ? Ok() : BadRequest("Already read or not found.");
    }

    [HttpGet("groups/{groupId}/messages")]
    public async Task<ActionResult<List<MessageDto>>> GetMessages(Guid groupId, [FromQuery] int skip = 0, [FromQuery] int take = 50, CancellationToken cancellationToken = default)
    {
        var results = await _chatManager.GetGroupMessagesAsync(groupId, skip, take, cancellationToken);
        return Ok(results);
    }

    [HttpGet("groups/{groupId}/unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount(Guid groupId, CancellationToken cancellationToken)
    {
        var count = await _chatManager.GetUnreadCountAsync(GetUserId(), groupId, cancellationToken);
        return Ok(count);
    }
}
