using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore.Extensions;

namespace SignalForge.EntityFrameworkCore.Configurations.ActivityLogConfigurations;

public class ActivityLogConfiguration : IEntityTypeConfiguration<Entities.ActivityLog>
{
    public void Configure(EntityTypeBuilder<Entities.ActivityLog> builder)
    {
        builder.ToTable(builder.GetTableName());
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ActivityType).IsRequired().HasMaxLength(128);
        builder.Property(x => x.UserId).HasMaxLength(128);
        builder.Property(x => x.IpAddress).HasMaxLength(64);
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}
