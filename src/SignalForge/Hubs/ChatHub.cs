using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalForge.Managers.Chat;
using SignalForge.Managers.Presence;
using SignalForge.Models.Dtos;

namespace SignalForge.Hubs;

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    private readonly IPresenceManager _presenceManager;
    private readonly IChatManager _chatManager;

    public ChatHub(IPresenceManager presenceManager, IChatManager chatManager)
    {
        _presenceManager = presenceManager;
        _chatManager = chatManager;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        if (userId != null)
        {
            await _presenceManager.UserConnectedAsync(userId.Value, Context.ConnectionId);
            
            // Broadcast presence
            var presence = await _presenceManager.GetUserPresenceAsync(userId.Value);
            if (presence != null)
            {
                await Clients.Others.PresenceUpdated(presence);
            }
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        if (userId != null)
        {
            await _presenceManager.UserDisconnectedAsync(userId.Value, Context.ConnectionId);
            
            var presence = await _presenceManager.GetUserPresenceAsync(userId.Value);
            if (presence != null)
            {
                await Clients.Others.PresenceUpdated(presence);
            }
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinGroup(Guid groupId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
    }

    public async Task LeaveGroup(Guid groupId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString());
    }

    public async Task SendTyping(Guid groupId, string displayName)
    {
        await Clients.Group(groupId.ToString()).UserTyping(groupId, displayName);
    }

    public async Task SendStoppedTyping(Guid groupId, string displayName)
    {
        await Clients.Group(groupId.ToString()).UserStoppedTyping(groupId, displayName);
    }

    private Guid? GetUserId()
    {
        var userIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdStr, out var userId)) return userId;
        return null;
    }
}
