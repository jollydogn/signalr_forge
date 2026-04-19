using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;

namespace SignalForge.EntityFrameworkCore.Configurations.ChatGroupMember;

public class ChatGroupMemberConfiguration : IEntityTypeConfiguration<Entities.ChatGroupMember>
{
    private readonly string _tablePrefix;
    private readonly string? _schema;

    public ChatGroupMemberConfiguration(string tablePrefix, string? schema)
    {
        _tablePrefix = tablePrefix;
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<Entities.ChatGroupMember> builder)
    {
        builder.ToTable(_tablePrefix + "GroupMembers", _schema);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Nickname).HasMaxLength(128);
        builder.HasIndex(x => new { x.GroupId, x.UserId }).IsUnique();

        builder.HasOne(x => x.Group)
               .WithMany(x => x.Members)
               .HasForeignKey(x => x.GroupId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
               .WithMany(x => x.GroupMemberships)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
