using System;

namespace SignalForge.Models.Dtos;

public class ActivityLogDto
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public string ActivityType { get; set; } = default!;
    public string? Description { get; set; }
    public string? Metadata { get; set; }
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }
}
