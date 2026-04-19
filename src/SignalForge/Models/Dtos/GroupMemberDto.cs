using System;

namespace SignalForge.Models.Dtos;

public class GroupMemberDto
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }
    public string DisplayName { get; set; } = default!;
    public string? Nickname { get; set; }
    public bool IsMuted { get; set; }
    public DateTime JoinedAt { get; set; }
}
