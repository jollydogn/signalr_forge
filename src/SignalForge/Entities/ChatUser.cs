using System;
using System.Collections.Generic;

namespace SignalForge.Entities;

public class ChatUser
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Identifier mapped from the consumer's identity provider (e.g. IdentityServer, Keycloak).
    /// </summary>
    public string ExternalUserId { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public DateTime LastSeenAt { get; set; }

    public bool IsOnline { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<ChatGroupMember> GroupMemberships { get; set; } = new List<ChatGroupMember>();
    public virtual ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
    public virtual ICollection<MessageReadReceipt> ReadReceipts { get; set; } = new List<MessageReadReceipt>();
    public virtual ICollection<UserConnection> Connections { get; set; } = new List<UserConnection>();
}
