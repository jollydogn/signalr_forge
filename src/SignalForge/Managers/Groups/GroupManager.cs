using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore;
using SignalForge.Models.Dtos;

namespace SignalForge.Managers.Groups;

public class GroupManager : IGroupManager
{
    private readonly ISignalForgeDbContext _dbContext;

    public GroupManager(ISignalForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<GroupDto> CreateGroupAsync(Guid creatorId, CreateGroupDto input, CancellationToken cancellationToken = default)
    {
        var group = new ChatGroup
        {
            Id = Guid.NewGuid(),
            CreatedByUserId = creatorId,
            Name = input.Name,
            Description = input.Description,
            IsDirectMessage = input.IsDirectMessage,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.ChatGroups.AddAsync(group, cancellationToken);
        
        // Add creator as member automatically
        var member = new ChatGroupMember
        {
            Id = Guid.NewGuid(),
            GroupId = group.Id,
            UserId = creatorId,
            JoinedAt = DateTime.UtcNow,
            IsMuted = false
        };
        await _dbContext.ChatGroupMembers.AddAsync(member, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(group);
    }

    public virtual async Task<GroupDto> GetDirectMessageGroupAsync(Guid user1Id, Guid user2Id, CancellationToken cancellationToken = default)
    {
        // Find existing DM group
        var existingGroup = await _dbContext.ChatGroups
            .Where(g => g.IsDirectMessage)
            .Where(g => g.Members.Any(m => m.UserId == user1Id) && g.Members.Any(m => m.UserId == user2Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (existingGroup != null) return MapToDto(existingGroup);

        // Create new DM group
        var dmName = $"DM_{user1Id}_{user2Id}";
        var group = new ChatGroup
        {
            Id = Guid.NewGuid(),
            CreatedByUserId = user1Id,
            Name = dmName,
            IsDirectMessage = true,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.ChatGroups.AddAsync(group, cancellationToken);
        await _dbContext.ChatGroupMembers.AddAsync(new ChatGroupMember { Id = Guid.NewGuid(), GroupId = group.Id, UserId = user1Id, JoinedAt = DateTime.UtcNow }, cancellationToken);
        await _dbContext.ChatGroupMembers.AddAsync(new ChatGroupMember { Id = Guid.NewGuid(), GroupId = group.Id, UserId = user2Id, JoinedAt = DateTime.UtcNow }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(group);
    }

    public virtual async Task<bool> JoinGroupAsync(Guid userId, Guid groupId, JoinGroupDto input, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.ChatGroupMembers.AnyAsync(m => m.GroupId == groupId && m.UserId == userId, cancellationToken);
        if (exists) return true;

        var member = new ChatGroupMember
        {
            Id = Guid.NewGuid(),
            GroupId = groupId,
            UserId = userId,
            Nickname = input.Nickname,
            JoinedAt = DateTime.UtcNow
        };

        await _dbContext.ChatGroupMembers.AddAsync(member, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public virtual async Task<bool> LeaveGroupAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default)
    {
        var member = await _dbContext.ChatGroupMembers.FirstOrDefaultAsync(m => m.GroupId == groupId && m.UserId == userId, cancellationToken);
        if (member == null) return false;

        _dbContext.ChatGroupMembers.Remove(member);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public virtual async Task<GroupDto?> GetGroupInfoAsync(Guid groupId, CancellationToken cancellationToken = default)
    {
        var group = await _dbContext.ChatGroups.FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
        return group == null ? null : MapToDto(group);
    }

    public virtual async Task<List<GroupMemberDto>> GetGroupMembersAsync(Guid groupId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ChatGroupMembers
            .Include(m => m.User)
            .Where(m => m.GroupId == groupId)
            .Select(m => new GroupMemberDto
            {
                GroupId = m.GroupId,
                UserId = m.UserId,
                DisplayName = m.User.DisplayName,
                Nickname = m.Nickname,
                IsMuted = m.IsMuted,
                JoinedAt = m.JoinedAt
            })
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<GroupDto>> GetUserGroupsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ChatGroupMembers
            .Where(m => m.UserId == userId)
            .Select(m => m.Group)
            .Select(g => MapToDto(g))
            .ToListAsync(cancellationToken);
    }

    private static GroupDto MapToDto(ChatGroup g) => new GroupDto
    {
        Id = g.Id,
        Name = g.Name,
        Description = g.Description,
        IsDirectMessage = g.IsDirectMessage,
        CreatedAt = g.CreatedAt
    };
}
