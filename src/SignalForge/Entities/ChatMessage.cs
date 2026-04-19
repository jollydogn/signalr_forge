using System;
using System.Collections.Generic;
using SignalForge.Models.Enums;

namespace SignalForge.Entities;

public class ChatMessage
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }
    public virtual ChatGroup Group { get; set; } = default!;

    public Guid SenderUserId { get; set; }
    public virtual ChatUser Sender { get; set; } = default!;

    public string Content { get; set; } = default!;

    public MessageStatus Status { get; set; }

    public DateTime SentAt { get; set; }

    public DateTime? EditedAt { get; set; }

    public bool IsDeleted { get; set; }

    // Navigation properties
    public virtual ICollection<MessageReadReceipt> ReadReceipts { get; set; } = new List<MessageReadReceipt>();
}
