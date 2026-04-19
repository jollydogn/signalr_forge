using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore.Extensions;

namespace SignalForge.EntityFrameworkCore.Configurations.ChatMessageConfigurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<Entities.ChatMessage>
{
    public void Configure(EntityTypeBuilder<Entities.ChatMessage> builder)
    {
        builder.ToTable(builder.GetTableName());
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
