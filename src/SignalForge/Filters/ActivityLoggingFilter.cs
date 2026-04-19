using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalForge.Configuration;
using SignalForge.Managers.Logging;
using SignalForge.Models.Dtos;

namespace SignalForge.Filters;

public class ActivityLoggingFilter : IAsyncActionFilter
{
    private readonly SignalForgeOptions _options;
    private readonly ILogger<ActivityLoggingFilter> _logger;

    public ActivityLoggingFilter(IOptions<SignalForgeOptions> options, ILogger<ActivityLoggingFilter> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Let the action execute first
        var resultContext = await next();

        if (!_options.EnableActivityLogging) return;

        // Check if action was successful (not unhandled exception)
        if (resultContext.Exception != null && !resultContext.ExceptionHandled) return;

        try
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor != null)
            {
                var attribute = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(ActivityLoggingAttribute), true).FirstOrDefault() as ActivityLoggingAttribute
                             ?? actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(ActivityLoggingAttribute), true).FirstOrDefault() as ActivityLoggingAttribute;

                if (attribute != null)
                {
                    var logManager = context.HttpContext.RequestServices.GetService<ILogManager>();
                    if (logManager != null)
                    {
                        var log = new ActivityLogDto
                        {
                            ActivityType = attribute.ActivityType,
                            Description = attribute.Description ?? $"Executed {actionDescriptor.ActionName}",
                            IpAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString(),
                            UserId = context.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        };

                        await logManager.LogActivityAsync(log);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log activity via SignalForge ActivityLoggingFilter.");
        }
    }
}
