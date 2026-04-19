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
        // services.AddScoped<IChatManager, ChatManager>();
        // services.AddScoped<IGroupManager, GroupManager>();
        // services.AddScoped<IPresenceManager, PresenceManager>();
        // services.AddScoped<ILogManager, LogManager>();

        return services;
    }

    /// <summary>
    /// Maps the internal SignalR Hub to the application router.
    /// Needs to be called in Program.cs (e.g., app.MapSignalForgeHub())
    /// </summary>
    public static IEndpointConventionBuilder MapSignalForgeHub(this IEndpointRouteBuilder endpoints)
    {
        // Hubs will be mapped here (Phase 3)
        // return endpoints.MapHub<ChatHub>("/hubs/chat");
        
        throw new NotImplementedException("ChatHub is not yet implemented.");
    }
}
