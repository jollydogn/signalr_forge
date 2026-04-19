using System;

namespace SignalForge.Entities;

public class UserConnection
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public virtual ChatUser User { get; set; } = default!;

    /// <summary>
    /// SignalR Connection ID
    /// </summary>
    public string ConnectionId { get; set; } = default!;

    public DateTime ConnectedAt { get; set; }

    public DateTime? DisconnectedAt { get; set; }
}
