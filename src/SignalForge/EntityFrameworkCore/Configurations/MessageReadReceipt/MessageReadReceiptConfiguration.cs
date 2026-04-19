using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;

namespace SignalForge.EntityFrameworkCore.Configurations.MessageReadReceipt;

public class MessageReadReceiptConfiguration : IEntityTypeConfiguration<Entities.MessageReadReceipt>
{
    private readonly string _tablePrefix;
    private readonly string? _schema;

    public MessageReadReceiptConfiguration(string tablePrefix, string? schema)
    {
        _tablePrefix = tablePrefix;
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<Entities.MessageReadReceipt> builder)
    {
        builder.ToTable(_tablePrefix + "MessageReadReceipts", _schema);
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => new { x.MessageId, x.UserId }).IsUnique();

        builder.HasOne(x => x.Message)
               .WithMany(x => x.ReadReceipts)
               .HasForeignKey(x => x.MessageId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
               .WithMany(x => x.ReadReceipts)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
