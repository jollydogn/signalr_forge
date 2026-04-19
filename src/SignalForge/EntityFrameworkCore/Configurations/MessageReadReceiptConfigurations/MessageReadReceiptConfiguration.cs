using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore.Extensions;

namespace SignalForge.EntityFrameworkCore.Configurations.MessageReadReceiptConfigurations;

public class MessageReadReceiptConfiguration : IEntityTypeConfiguration<Entities.MessageReadReceipt>
{
    public void Configure(EntityTypeBuilder<Entities.MessageReadReceipt> builder)
    {
        builder.ToTable(builder.GetTableName());
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
