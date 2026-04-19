using Microsoft.EntityFrameworkCore;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore;

namespace SignalForge.Sample;

public class SampleDbContext : DbContext, ISignalForgeDbContext
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
    {
    }

    public DbSet<ChatUser> ChatUsers { get; set; } = default!;
    public DbSet<ChatGroup> ChatGroups { get; set; } = default!;
    public DbSet<ChatGroupMember> ChatGroupMembers { get; set; } = default!;
    public DbSet<ChatMessage> ChatMessages { get; set; } = default!;
    public DbSet<MessageReadReceipt> MessageReadReceipts { get; set; } = default!;
    public DbSet<UserConnection> UserConnections { get; set; } = default!;
    public DbSet<RequestLog> RequestLogs { get; set; } = default!;
    public DbSet<ActivityLog> ActivityLogs { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // This is the magic linking SignalForge tables into your context
        modelBuilder.ConfigureSignalForge();
    }
}
