using System;
using System.Threading.Tasks;
using SignalForge.Models.Dtos;

namespace SignalForge.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(MessageDto message);
    Task MessageEdited(MessageDto message);
    Task MessageDeleted(Guid messageId);
    Task MessageRead(Guid messageId, Guid userId);
    
    Task UserJoinedGroup(Guid groupId, string userDisplayName);
    Task UserLeftGroup(Guid groupId, string userDisplayName);
    
    Task UserTyping(Guid groupId, string userDisplayName);
    Task UserStoppedTyping(Guid groupId, string userDisplayName);
    
    Task PresenceUpdated(UserPresenceDto presence);
}
