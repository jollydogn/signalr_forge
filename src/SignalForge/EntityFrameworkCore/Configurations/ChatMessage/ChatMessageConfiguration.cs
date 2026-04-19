using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;

namespace SignalForge.EntityFrameworkCore.Configurations.ChatMessage;

public class ChatMessageConfiguration : IEntityTypeConfiguration<Entities.ChatMessage>
{
    private readonly string _tablePrefix;
    private readonly string? _schema;

    public ChatMessageConfiguration(string tablePrefix, string? schema)
    {
        _tablePrefix = tablePrefix;
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<Entities.ChatMessage> builder)
    {
        builder.ToTable(_tablePrefix + "Messages", _schema);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Content).IsRequired().HasMaxLength(4000);

        builder.HasOne(x => x.Group)
               .WithMany(x => x.Messages)
               .HasForeignKey(x => x.GroupId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Sender)
               .WithMany(x => x.SentMessages)
               .HasForeignKey(x => x.SenderUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
