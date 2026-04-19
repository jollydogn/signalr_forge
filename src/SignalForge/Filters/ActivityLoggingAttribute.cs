using System;

namespace SignalForge.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ActivityLoggingAttribute : Attribute
{
    public string ActivityType { get; }
    public string? Description { get; set; }

    public ActivityLoggingAttribute(string activityType)
    {
        ActivityType = activityType;
    }
}
