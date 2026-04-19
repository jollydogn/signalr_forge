using System;

namespace SignalForge.Entities;

public class MessageReadReceipt
{
    public Guid Id { get; set; }

    public Guid MessageId { get; set; }
    public virtual ChatMessage Message { get; set; } = default!;

    public Guid UserId { get; set; }
    public virtual ChatUser User { get; set; } = default!;

    public DateTime ReadAt { get; set; }
}
