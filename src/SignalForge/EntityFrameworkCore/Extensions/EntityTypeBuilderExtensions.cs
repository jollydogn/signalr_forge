using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;

namespace SignalForge.EntityFrameworkCore.Extensions;

public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Returns the corresponding database table name for the given Entity type.
    /// You can freely edit these names here to dynamically map across your SQL tables.
    /// </summary>
    public static string GetTableName<T>(this EntityTypeBuilder<T> builder) where T : class
    {
        var type = typeof(T);

        if (type == typeof(ChatUser)) return "Users";
        if (type == typeof(ChatGroup)) return "Groups";
        if (type == typeof(ChatGroupMember)) return "GroupMembers";
        if (type == typeof(ChatMessage)) return "Messages";
        if (type == typeof(MessageReadReceipt)) return "MessageReadReceipts";
        if (type == typeof(UserConnection)) return "UserConnections";
        if (type == typeof(RequestLog)) return "RequestLogs";
        if (type == typeof(ActivityLog)) return "ActivityLogs";

        // Fallback for any other entities
        return type.Name + "s";
    }
}
