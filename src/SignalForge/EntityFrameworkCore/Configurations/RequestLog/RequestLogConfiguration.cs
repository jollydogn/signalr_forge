using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;

namespace SignalForge.EntityFrameworkCore.Configurations.RequestLog;

public class RequestLogConfiguration : IEntityTypeConfiguration<Entities.RequestLog>
{
    private readonly string _tablePrefix;
    private readonly string? _schema;

    public RequestLogConfiguration(string tablePrefix, string? schema)
    {
        _tablePrefix = tablePrefix;
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<Entities.RequestLog> builder)
    {
        builder.ToTable(_tablePrefix + "RequestLogs", _schema);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.HttpMethod).HasMaxLength(16);
        builder.Property(x => x.Path).HasMaxLength(500);
        builder.Property(x => x.UserAgent).HasMaxLength(500);
        builder.Property(x => x.IpAddress).HasMaxLength(64);
        builder.Property(x => x.UserId).HasMaxLength(128);
    }
}
