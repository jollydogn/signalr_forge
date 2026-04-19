using System;

namespace SignalForge.Models.Dtos;

public class RequestLogDto
{
    public Guid Id { get; set; }
    public string HttpMethod { get; set; } = default!;
    public string Path { get; set; } = default!;
    public int StatusCode { get; set; }
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
    public string? UserId { get; set; }
    public long ElapsedMs { get; set; }
    public DateTime CreatedAt { get; set; }
}
