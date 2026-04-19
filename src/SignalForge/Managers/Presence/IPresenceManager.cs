using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignalForge.Models.Dtos;

namespace SignalForge.Managers.Presence;

public interface IPresenceManager
{
    Task<bool> UserConnectedAsync(Guid userId, string connectionId, CancellationToken cancellationToken = default);
    Task<bool> UserDisconnectedAsync(Guid userId, string connectionId, CancellationToken cancellationToken = default);
    Task<UserPresenceDto?> GetUserPresenceAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<List<UserPresenceDto>> GetOnlineUsersAsync(CancellationToken cancellationToken = default);
    Task<List<UserPresenceDto>> GetGroupPresenceAsync(Guid groupId, CancellationToken cancellationToken = default);
}
