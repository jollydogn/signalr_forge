using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignalForge.Models.Dtos;

namespace SignalForge.Managers.Groups;

public interface IGroupManager
{
    Task<GroupDto> CreateGroupAsync(Guid creatorId, CreateGroupDto input, CancellationToken cancellationToken = default);
    Task<GroupDto> GetDirectMessageGroupAsync(Guid user1Id, Guid user2Id, CancellationToken cancellationToken = default);
    Task<bool> JoinGroupAsync(Guid userId, Guid groupId, JoinGroupDto input, CancellationToken cancellationToken = default);
    Task<bool> LeaveGroupAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default);
    Task<GroupDto?> GetGroupInfoAsync(Guid groupId, CancellationToken cancellationToken = default);
    Task<List<GroupMemberDto>> GetGroupMembersAsync(Guid groupId, CancellationToken cancellationToken = default);
    Task<List<GroupDto>> GetUserGroupsAsync(Guid userId, CancellationToken cancellationToken = default);
}
