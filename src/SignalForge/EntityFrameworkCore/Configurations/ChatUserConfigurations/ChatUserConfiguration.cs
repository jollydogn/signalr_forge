using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore.Extensions;

namespace SignalForge.EntityFrameworkCore.Configurations.ChatUserConfigurations;

public class ChatUserConfiguration : IEntityTypeConfiguration<Entities.ChatUser>
{
    public void Configure(EntityTypeBuilder<Entities.ChatUser> builder)
    {
        builder.ToTable(builder.GetTableName());
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ExternalUserId).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.ExternalUserId).IsUnique();
        
        builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(128);
    }
}
