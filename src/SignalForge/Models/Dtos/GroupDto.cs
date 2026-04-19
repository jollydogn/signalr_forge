using System;

namespace SignalForge.Models.Dtos;

public class GroupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsDirectMessage { get; set; }
    public DateTime CreatedAt { get; set; }
}
