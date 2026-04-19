using System;

namespace SignalForge.Entities;

public class ActivityLog
{
    public Guid Id { get; set; }

    public string? UserId { get; set; }

    /// <summary>
    /// Type of activity (e.g., 'MessageSent', 'UserLogin')
    /// </summary>
    public string ActivityType { get; set; } = default!;

    public string? Description { get; set; }

    /// <summary>
    /// JSON payload with extra contextual data
    /// </summary>
    public string? Metadata { get; set; }

    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; }
}
