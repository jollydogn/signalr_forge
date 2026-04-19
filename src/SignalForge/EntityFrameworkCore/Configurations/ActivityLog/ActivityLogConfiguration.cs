using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;

namespace SignalForge.EntityFrameworkCore.Configurations.ActivityLog;

public class ActivityLogConfiguration : IEntityTypeConfiguration<Entities.ActivityLog>
{
    private readonly string _tablePrefix;
    private readonly string? _schema;

    public ActivityLogConfiguration(string tablePrefix, string? schema)
    {
        _tablePrefix = tablePrefix;
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<Entities.ActivityLog> builder)
    {
        builder.ToTable(_tablePrefix + "ActivityLogs", _schema);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ActivityType).IsRequired().HasMaxLength(128);
        builder.Property(x => x.UserId).HasMaxLength(128);
        builder.Property(x => x.IpAddress).HasMaxLength(64);
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}
