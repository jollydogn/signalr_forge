using System;

namespace SignalForge.Models.Dtos;

public class SendMessageDto
{
    public Guid GroupId { get; set; }
    public string Content { get; set; } = default!;
}
