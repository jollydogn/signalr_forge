using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;

namespace SignalForge.EntityFrameworkCore.Configurations.ChatUser;

public class ChatUserConfiguration : IEntityTypeConfiguration<Entities.ChatUser>
{
    private readonly string _tablePrefix;
    private readonly string? _schema;

    public ChatUserConfiguration(string tablePrefix, string? schema)
    {
        _tablePrefix = tablePrefix;
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<Entities.ChatUser> builder)
    {
        builder.ToTable(_tablePrefix + "Users", _schema);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ExternalUserId).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.ExternalUserId).IsUnique();
        
        builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(128);
    }
}
