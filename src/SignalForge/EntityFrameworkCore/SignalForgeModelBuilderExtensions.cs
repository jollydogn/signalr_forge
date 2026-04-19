using Microsoft.EntityFrameworkCore;
using SignalForge.Entities;

namespace SignalForge.EntityFrameworkCore;

public static class SignalForgeModelBuilderExtensions
{
    /// <summary>
    /// Configures the EF Core Model Builder to include all SignalForge chat and logging tables.
    /// Call this within your DbContext's OnModelCreating method.
    /// </summary>
    public static void ConfigureSignalForge(this ModelBuilder builder, string tablePrefix = "Sf_", string? schema = null)
    {
        // ── ChatUser ──
        builder.Entity<ChatUser>(b =>
        {
            b.ToTable(tablePrefix + "Users", schema);
            b.HasKey(x => x.Id);
            b.Property(x => x.ExternalUserId).IsRequired().HasMaxLength(256);
            b.HasIndex(x => x.ExternalUserId).IsUnique();
            b.Property(x => x.DisplayName).IsRequired().HasMaxLength(128);
        });

        // ── ChatGroup ──
        builder.Entity<ChatGroup>(b =>
        {
            b.ToTable(tablePrefix + "Groups", schema);
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.Description).HasMaxLength(500);

            b.HasOne(x => x.CreatedBy)
             .WithMany()
             .HasForeignKey(x => x.CreatedByUserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── ChatGroupMember ──
        builder.Entity<ChatGroupMember>(b =>
        {
            b.ToTable(tablePrefix + "GroupMembers", schema);
            b.HasKey(x => x.Id);
            b.Property(x => x.Nickname).HasMaxLength(128);
            b.HasIndex(x => new { x.GroupId, x.UserId }).IsUnique();

            b.HasOne(x => x.Group)
             .WithMany(x => x.Members)
             .HasForeignKey(x => x.GroupId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.User)
             .WithMany(x => x.GroupMemberships)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── ChatMessage ──
        builder.Entity<ChatMessage>(b =>
        {
            b.ToTable(tablePrefix + "Messages", schema);
            b.HasKey(x => x.Id);
            b.Property(x => x.Content).IsRequired().HasMaxLength(4000);

            b.HasOne(x => x.Group)
             .WithMany(x => x.Messages)
             .HasForeignKey(x => x.GroupId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Sender)
             .WithMany(x => x.SentMessages)
             .HasForeignKey(x => x.SenderUserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── MessageReadReceipt ──
        builder.Entity<MessageReadReceipt>(b =>
        {
            b.ToTable(tablePrefix + "MessageReadReceipts", schema);
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.MessageId, x.UserId }).IsUnique();

            b.HasOne(x => x.Message)
             .WithMany(x => x.ReadReceipts)
             .HasForeignKey(x => x.MessageId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.User)
             .WithMany(x => x.ReadReceipts)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── UserConnection ──
        builder.Entity<UserConnection>(b =>
        {
            b.ToTable(tablePrefix + "UserConnections", schema);
            b.HasKey(x => x.Id);
            b.Property(x => x.ConnectionId).IsRequired().HasMaxLength(128);
            b.HasIndex(x => x.ConnectionId).IsUnique();
            b.HasIndex(x => x.DisconnectedAt); // Often queried for active connections

            b.HasOne(x => x.User)
             .WithMany(x => x.Connections)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── RequestLog ──
        builder.Entity<RequestLog>(b =>
        {
            b.ToTable(tablePrefix + "RequestLogs", schema);
            b.HasKey(x => x.Id);
            b.Property(x => x.HttpMethod).HasMaxLength(16);
            b.Property(x => x.Path).HasMaxLength(500);
            b.Property(x => x.UserAgent).HasMaxLength(500);
            b.Property(x => x.IpAddress).HasMaxLength(64);
            b.Property(x => x.UserId).HasMaxLength(128);
        });

        // ── ActivityLog ──
        builder.Entity<ActivityLog>(b =>
        {
            b.ToTable(tablePrefix + "ActivityLogs", schema);
            b.HasKey(x => x.Id);
            b.Property(x => x.ActivityType).IsRequired().HasMaxLength(128);
            b.Property(x => x.UserId).HasMaxLength(128);
            b.Property(x => x.IpAddress).HasMaxLength(64);
            b.Property(x => x.Description).HasMaxLength(1000);
            // Metadata is unrestricted (or large text)
        });
    }
}
