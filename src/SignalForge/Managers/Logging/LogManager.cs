using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore;
using SignalForge.Models.Dtos;

namespace SignalForge.Managers.Logging;

public class LogManager : ILogManager
{
    private readonly ISignalForgeDbContext _dbContext;

    public LogManager(ISignalForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task LogRequestAsync(RequestLogDto log, CancellationToken cancellationToken = default)
    {
        var entity = new RequestLog
        {
            Id = Guid.NewGuid(),
            HttpMethod = log.HttpMethod,
            Path = log.Path,
            StatusCode = log.StatusCode,
            RequestBody = log.RequestBody,
            ResponseBody = log.ResponseBody,
            UserAgent = log.UserAgent,
            IpAddress = log.IpAddress,
            UserId = log.UserId,
            ElapsedMs = log.ElapsedMs,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.RequestLogs.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task LogActivityAsync(ActivityLogDto log, CancellationToken cancellationToken = default)
    {
        var entity = new ActivityLog
        {
            Id = Guid.NewGuid(),
            UserId = log.UserId,
            ActivityType = log.ActivityType,
            Description = log.Description,
            Metadata = log.Metadata,
            IpAddress = log.IpAddress,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.ActivityLogs.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<List<RequestLogDto>> GetRecentRequestsAsync(int count = 100, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RequestLogs
            .OrderByDescending(r => r.CreatedAt)
            .Take(count)
            .Select(r => new RequestLogDto
            {
                Id = r.Id,
                HttpMethod = r.HttpMethod,
                Path = r.Path,
                StatusCode = r.StatusCode,
                RequestBody = r.RequestBody,
                ResponseBody = r.ResponseBody,
                UserAgent = r.UserAgent,
                IpAddress = r.IpAddress,
                UserId = r.UserId,
                ElapsedMs = r.ElapsedMs,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<ActivityLogDto>> GetUserActivitiesAsync(string userId, int count = 50, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ActivityLogs
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .Select(a => new ActivityLogDto
            {
                Id = a.Id,
                UserId = a.UserId,
                ActivityType = a.ActivityType,
                Description = a.Description,
                Metadata = a.Metadata,
                IpAddress = a.IpAddress,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
