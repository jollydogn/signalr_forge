using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore.Extensions;

namespace SignalForge.EntityFrameworkCore.Configurations.ChatGroupMemberConfigurations;

public class ChatGroupMemberConfiguration : IEntityTypeConfiguration<Entities.ChatGroupMember>
{
    public void Configure(EntityTypeBuilder<Entities.ChatGroupMember> builder)
    {
        builder.ToTable(builder.GetTableName());
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
