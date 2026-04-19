using Microsoft.EntityFrameworkCore;
using SignalForge.Entities;

namespace SignalForge.EntityFrameworkCore;

/// <summary>
/// Interface that the consumer's DbContext should implement or the Managers can use directly
/// if registered. Alternatively, consumers can just register this interface to their DbContext.
/// </summary>
public interface ISignalForgeDbContext
{
    DbSet<ChatUser> ChatUsers { get; set; }
    DbSet<ChatGroup> ChatGroups { get; set; }
    DbSet<ChatGroupMember> ChatGroupMembers { get; set; }
    DbSet<ChatMessage> ChatMessages { get; set; }
    DbSet<MessageReadReceipt> MessageReadReceipts { get; set; }
    DbSet<UserConnection> UserConnections { get; set; }
    DbSet<RequestLog> RequestLogs { get; set; }
    DbSet<ActivityLog> ActivityLogs { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
