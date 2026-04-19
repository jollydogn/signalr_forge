using System;
using SignalForge.Models.Enums;

namespace SignalForge.Models.Dtos;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    
    public Guid SenderUserId { get; set; }
    public string SenderDisplayName { get; set; } = default!;
    
    public string Content { get; set; } = default!;
    public MessageStatus Status { get; set; }
    
    public DateTime SentAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public bool IsDeleted { get; set; }
}
