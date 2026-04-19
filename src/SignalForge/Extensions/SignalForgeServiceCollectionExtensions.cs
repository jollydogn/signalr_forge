using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalForge.Configuration;

namespace SignalForge.Extensions;

public static class SignalForgeServiceCollectionExtensions
{
    /// <summary>
    /// Core registration for SignalForge business logic and SignalR services.
    /// </summary>
    public static IServiceCollection AddSignalForge(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "SignalForge",
        Action<SignalForgeOptions>? setupAction = null)
    {
        var section = configuration.GetSection(sectionName);
        services.Configure<SignalForgeOptions>(section);

        if (setupAction != null)
        {
            services.Configure(setupAction);
        }

        var options = new SignalForgeOptions();
        section.Bind(options);
        setupAction?.Invoke(options);

        // Required SignalR registration
        services.AddSignalR(hubOptions =>
        {
            hubOptions.EnableDetailedErrors = options.EnableDetailedErrors;
        });

        // Add HttpContextAccessor for global filters and user resolution
        services.AddHttpContextAccessor();

        // Managers will be registered here (Phase 2)
        services.AddScoped<SignalForge.Managers.Chat.IChatManager, SignalForge.Managers.Chat.ChatManager>();
        services.AddScoped<SignalForge.Managers.Groups.IGroupManager, SignalForge.Managers.Groups.GroupManager>();
        services.AddScoped<SignalForge.Managers.Presence.IPresenceManager, SignalForge.Managers.Presence.PresenceManager>();
        services.AddScoped<SignalForge.Managers.Logging.ILogManager, SignalForge.Managers.Logging.LogManager>();

        return services;
    }

    /// <summary>
    /// Maps the internal SignalR Hub to the application router.
    /// Needs to be called in Program.cs (e.g., app.MapSignalForgeHub())
    /// </summary>
    public static IEndpointConventionBuilder MapSignalForgeHub(this IEndpointRouteBuilder endpoints)
    {
        var options = endpoints.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<SignalForgeOptions>>().Value;
        return endpoints.MapHub<SignalForge.Hubs.ChatHub>(options.HubPath);
    }
}
