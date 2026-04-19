using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace SignalForge.EntityFrameworkCore.Extensions;

public static class ModelBuilderSnakeCaseExtensions
{
    public static void ToSnakeCase(this ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (tableName != null)
                entity.SetTableName(tableName.ToSnakeCase());

            foreach (var property in entity.GetProperties())
            {
                var columnName = property.GetColumnName();
                if (columnName != null)
                    property.SetColumnName(columnName.ToSnakeCase());
            }

            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (keyName != null)
                    key.SetName(keyName.ToSnakeCase());
            }

            foreach (var key in entity.GetForeignKeys())
            {
                var foreignKeyName = key.GetConstraintName();
                if (foreignKeyName != null)
                    key.SetConstraintName(foreignKeyName.ToSnakeCase());
            }

            foreach (var index in entity.GetIndexes())
            {
                var indexName = index.GetDatabaseName();
                if (indexName != null)
                    index.SetDatabaseName(indexName.ToSnakeCase());
            }
        }
    }

    private static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}
