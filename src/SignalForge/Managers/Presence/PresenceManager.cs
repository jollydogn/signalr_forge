using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore;
using SignalForge.Models.Dtos;

namespace SignalForge.Managers.Presence;

public class PresenceManager : IPresenceManager
{
    private readonly ISignalForgeDbContext _dbContext;

    public PresenceManager(ISignalForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<bool> UserConnectedAsync(Guid userId, string connectionId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.ChatUsers.FindAsync(new object[] { userId }, cancellationToken);
        if (user == null) return false;

        var connection = new UserConnection
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ConnectionId = connectionId,
            ConnectedAt = DateTime.UtcNow
        };

        if (!user.IsOnline)
        {
            user.IsOnline = true;
        }

        await _dbContext.UserConnections.AddAsync(connection, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public virtual async Task<bool> UserDisconnectedAsync(Guid userId, string connectionId, CancellationToken cancellationToken = default)
    {
        var connection = await _dbContext.UserConnections
            .FirstOrDefaultAsync(c => c.ConnectionId == connectionId, cancellationToken);

        if (connection != null)
        {
            connection.DisconnectedAt = DateTime.UtcNow;
            
            // Check if user still has other active connections
            var hasActiveConnections = await _dbContext.UserConnections
                .AnyAsync(c => c.UserId == userId && c.DisconnectedAt == null && c.Id != connection.Id, cancellationToken);

            if (!hasActiveConnections)
            {
                var user = await _dbContext.ChatUsers.FindAsync(new object[] { userId }, cancellationToken);
                if (user != null)
                {
                    user.IsOnline = false;
                    user.LastSeenAt = DateTime.UtcNow;
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return true;
    }

    public virtual async Task<UserPresenceDto?> GetUserPresenceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.ChatUsers.FindAsync(new object[] { userId }, cancellationToken);
        if (user == null) return null;

        return new UserPresenceDto
        {
            UserId = user.Id,
            DisplayName = user.DisplayName,
            IsOnline = user.IsOnline,
            LastSeenAt = user.LastSeenAt
        };
    }

    public virtual async Task<List<UserPresenceDto>> GetOnlineUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.ChatUsers
            .Where(u => u.IsOnline)
            .Select(u => new UserPresenceDto
            {
                UserId = u.Id,
                DisplayName = u.DisplayName,
                IsOnline = true,
                LastSeenAt = u.LastSeenAt
            })
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<UserPresenceDto>> GetGroupPresenceAsync(Guid groupId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ChatGroupMembers
            .Where(m => m.GroupId == groupId)
            .Select(m => m.User)
            .Select(u => new UserPresenceDto
            {
                UserId = u.Id,
                DisplayName = u.DisplayName,
                IsOnline = u.IsOnline,
                LastSeenAt = u.LastSeenAt
            })
            .ToListAsync(cancellationToken);
    }
}
