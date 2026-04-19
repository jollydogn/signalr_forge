using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore.Extensions;

namespace SignalForge.EntityFrameworkCore.Configurations.UserConnectionConfigurations;

public class UserConnectionConfiguration : IEntityTypeConfiguration<Entities.UserConnection>
{
    public void Configure(EntityTypeBuilder<Entities.UserConnection> builder)
    {
        builder.ToTable(builder.GetTableName());
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ConnectionId).IsRequired().HasMaxLength(128);
        builder.HasIndex(x => x.ConnectionId).IsUnique();
        builder.HasIndex(x => x.DisconnectedAt);

        builder.HasOne(x => x.User)
               .WithMany(x => x.Connections)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
