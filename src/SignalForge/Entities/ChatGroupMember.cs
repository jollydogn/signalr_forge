using System;

namespace SignalForge.Entities;

public class ChatGroupMember
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }
    public virtual ChatGroup Group { get; set; } = default!;

    public Guid UserId { get; set; }
    public virtual ChatUser User { get; set; } = default!;

    /// <summary>
    /// Optional nickname specific to this group only.
    /// </summary>
    public string? Nickname { get; set; }

    public DateTime JoinedAt { get; set; }

    public bool IsMuted { get; set; }
}
