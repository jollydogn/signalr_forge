using System.Collections.Generic;

namespace SignalForge.Models.Dtos;

public class CreateGroupDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsDirectMessage { get; set; }
    
    // Optional: add multiple members on creation
    public List<string> InitialMembersExternalIds { get; set; } = new();
}
