using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalForge.Entities;

namespace SignalForge.EntityFrameworkCore.Configurations.UserConnection;

public class UserConnectionConfiguration : IEntityTypeConfiguration<Entities.UserConnection>
{
    private readonly string _tablePrefix;
    private readonly string? _schema;

    public UserConnectionConfiguration(string tablePrefix, string? schema)
    {
        _tablePrefix = tablePrefix;
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<Entities.UserConnection> builder)
    {
        builder.ToTable(_tablePrefix + "UserConnections", _schema);
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
