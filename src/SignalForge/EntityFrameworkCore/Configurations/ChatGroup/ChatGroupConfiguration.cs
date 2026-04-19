using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;

namespace SignalForge.EntityFrameworkCore.Configurations.ChatGroup;

public class ChatGroupConfiguration : IEntityTypeConfiguration<Entities.ChatGroup>
{
    private readonly string _tablePrefix;
    private readonly string? _schema;

    public ChatGroupConfiguration(string tablePrefix, string? schema)
    {
        _tablePrefix = tablePrefix;
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<Entities.ChatGroup> builder)
    {
        builder.ToTable(_tablePrefix + "Groups", _schema);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(500);

        builder.HasOne(x => x.CreatedBy)
               .WithMany()
               .HasForeignKey(x => x.CreatedByUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
