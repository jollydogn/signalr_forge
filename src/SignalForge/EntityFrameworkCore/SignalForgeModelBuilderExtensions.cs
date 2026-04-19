using Microsoft.EntityFrameworkCore;

namespace SignalForge.EntityFrameworkCore;

public static class SignalForgeModelBuilderExtensions
{
    /// <summary>
    /// Configures the EF Core Model Builder to include all SignalForge chat and logging tables.
    /// Call this within your DbContext's OnModelCreating method.
    /// </summary>
    public static void ConfigureSignalForge(this ModelBuilder builder, string tablePrefix = "Sf_", string? schema = null)
    {
        builder.ApplyConfiguration(new Configurations.ChatUser.ChatUserConfiguration(tablePrefix, schema));
        builder.ApplyConfiguration(new Configurations.ChatGroup.ChatGroupConfiguration(tablePrefix, schema));
        builder.ApplyConfiguration(new Configurations.ChatGroupMember.ChatGroupMemberConfiguration(tablePrefix, schema));
        builder.ApplyConfiguration(new Configurations.ChatMessage.ChatMessageConfiguration(tablePrefix, schema));
        builder.ApplyConfiguration(new Configurations.MessageReadReceipt.MessageReadReceiptConfiguration(tablePrefix, schema));
        builder.ApplyConfiguration(new Configurations.UserConnection.UserConnectionConfiguration(tablePrefix, schema));
        builder.ApplyConfiguration(new Configurations.RequestLog.RequestLogConfiguration(tablePrefix, schema));
        builder.ApplyConfiguration(new Configurations.ActivityLog.ActivityLogConfiguration(tablePrefix, schema));
    }
}
