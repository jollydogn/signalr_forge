using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignalForge.Models.Dtos;

namespace SignalForge.Managers.Logging;

public interface ILogManager
{
    Task LogRequestAsync(RequestLogDto log, CancellationToken cancellationToken = default);
    Task LogActivityAsync(ActivityLogDto log, CancellationToken cancellationToken = default);
    Task<List<RequestLogDto>> GetRecentRequestsAsync(int count = 100, CancellationToken cancellationToken = default);
    Task<List<ActivityLogDto>> GetUserActivitiesAsync(string userId, int count = 50, CancellationToken cancellationToken = default);
}
