using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;
using SignalForge.EntityFrameworkCore.Extensions;

namespace SignalForge.EntityFrameworkCore.Configurations.RequestLogConfigurations;

public class RequestLogConfiguration : IEntityTypeConfiguration<Entities.RequestLog>
{
    public void Configure(EntityTypeBuilder<Entities.RequestLog> builder)
    {
        builder.ToTable(builder.GetTableName());
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.HttpMethod).HasMaxLength(16);
        builder.Property(x => x.Path).HasMaxLength(500);
        builder.Property(x => x.UserAgent).HasMaxLength(500);
        builder.Property(x => x.IpAddress).HasMaxLength(64);
        builder.Property(x => x.UserId).HasMaxLength(128);
    }
}
