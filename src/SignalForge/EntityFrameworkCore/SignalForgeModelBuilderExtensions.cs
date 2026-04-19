using Microsoft.EntityFrameworkCore;
using SignalForge.EntityFrameworkCore.Extensions;

namespace SignalForge.EntityFrameworkCore;

public static class SignalForgeModelBuilderExtensions
{
    /// <summary>
    /// Configures the EF Core Model Builder to include all SignalForge chat and logging tables.
    /// Call this within your DbContext's OnModelCreating method.
    /// </summary>
    public static void ConfigureSignalForge(this ModelBuilder builder, string tablePrefix = "Sf_", string? schema = null)
    {
        // 1. Read all configurations directly from the assembly automatically
        builder.ApplyConfigurationsFromAssembly(typeof(SignalForgeModelBuilderExtensions).Assembly);

        // 2. Apply Custom Table Prefix and Schema to all SignalForge entities dynamically
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            if (entity.ClrType.Namespace != null && entity.ClrType.Namespace.Contains("SignalForge.Entities"))
            {
                var tableName = entity.GetTableName();
                if (tableName != null && !tableName.StartsWith(tablePrefix))
                {
                    entity.SetTableName(tablePrefix + tableName);
                }

                if (!string.IsNullOrEmpty(schema))
                {
                    entity.SetSchema(schema);
                }
            }
        }

        // 3. Convert all Database names and columns to snake_case format
        builder.ToSnakeCase();
    }
}
