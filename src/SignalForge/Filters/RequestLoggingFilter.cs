using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalForge.Configuration;
using SignalForge.Managers.Logging;
using SignalForge.Models.Dtos;

namespace SignalForge.Filters;

public class RequestLoggingFilter : IAsyncActionFilter
{
    private readonly SignalForgeOptions _options;
    private readonly ILogger<RequestLoggingFilter> _logger;

    public RequestLoggingFilter(IOptions<SignalForgeOptions> options, ILogger<RequestLoggingFilter> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!_options.EnableRequestLogging)
        {
            await next();
            return;
        }

        var sw = Stopwatch.StartNew();

        // Let the action execute
        var resultContext = await next();

        sw.Stop();

        try
        {
            var logManager = context.HttpContext.RequestServices.GetService<ILogManager>();
            if (logManager != null)
            {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;

                var log = new RequestLogDto
                {
                    HttpMethod = request.Method,
                    Path = request.Path,
                    StatusCode = response.StatusCode,
                    UserAgent = request.Headers.UserAgent.ToString(),
                    IpAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    UserId = context.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    ElapsedMs = sw.ElapsedMilliseconds
                };

                await logManager.LogRequestAsync(log);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log request via SignalForge RequestLoggingFilter.");
        }
    }
}
