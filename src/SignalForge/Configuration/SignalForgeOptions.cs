namespace SignalForge.Configuration;

public class SignalForgeOptions
{
    /// <summary>
    /// Path where the SignalR hub will be mapped. Default: "/hubs/chat"
    /// </summary>
    public string HubPath { get; set; } = "/hubs/chat";

    /// <summary>
    /// Enable detailed errors for SignalR. Default: false.
    /// </summary>
    public bool EnableDetailedErrors { get; set; } = false;

    /// <summary>
    /// Should activity logging be globally enabled? Default: true
    /// </summary>
    public bool EnableActivityLogging { get; set; } = true;
    
    /// <summary>
    /// Should request logging be globally enabled? Default: true
    /// </summary>
    public bool EnableRequestLogging { get; set; } = true;
}
