using System;
using System.Collections.Generic;

namespace SignalForge.Entities;

public class ChatGroup
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    /// <summary>
    /// The user who created the group.
    /// </summary>
    public Guid CreatedByUserId { get; set; }
    public virtual ChatUser CreatedBy { get; set; } = default!;

    /// <summary>
    /// If true, this group is a direct 1:1 message channel (will have exactly 2 members).
    /// </summary>
    public bool IsDirectMessage { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<ChatGroupMember> Members { get; set; } = new List<ChatGroupMember>();
    public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}
