using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore.Extensions;

namespace SignalForge.EntityFrameworkCore.Configurations.ChatGroupConfigurations;

public class ChatGroupConfiguration : IEntityTypeConfiguration<Entities.ChatGroup>
{
    public void Configure(EntityTypeBuilder<Entities.ChatGroup> builder)
    {
        builder.ToTable(builder.GetTableName());
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(500);

        builder.HasOne(x => x.CreatedBy)
               .WithMany()
               .HasForeignKey(x => x.CreatedByUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
