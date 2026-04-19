using System;

namespace SignalForge.Models.Dtos;

public class UserPresenceDto
{
    public Guid UserId { get; set; }
    public string DisplayName { get; set; } = default!;
    public bool IsOnline { get; set; }
    public DateTime LastSeenAt { get; set; }
}
