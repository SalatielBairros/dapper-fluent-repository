using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Mapping;
using Dapper.FluentMap;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Dapper.Fluent.ORM.Dommel;

/// <summary>
/// Default implementation of the <see cref="ITableNameResolver"/> interface.
/// </summary>
public class DefaultTableNameResolver : ITableNameResolver
{
    private readonly ISchema _schema;

    public DefaultTableNameResolver(ISchema schema)
    {
        _schema = schema;
    }

    /// <summary>
    /// Resolves the table name.
    /// Looks for the [Table] attribute. Otherwise by making the type
    /// plural (eg. Product -> Products) and removing the 'I' prefix for interfaces.
    /// </summary>
    public string ResolveTableName(Type type)
    {
        if (!FluentMapper.EntityMaps.TryGetValue(type, out var entityMap))
            return ResolveTableNameFromReflection(type);

        if (!(entityMap is IDapperFluentEntityMap mapping))
            return ResolveTableNameFromReflection(type);

        var tableName = mapping.TableName;

        var schema = _schema.GetSchema();

        if (tableName.Contains(".") && mapping.IsDynamicSchema && !string.IsNullOrWhiteSpace(schema))
        {
            return $"{schema}.{tableName.Split(".")[1]}";
        }

        return tableName;
    }

    private string ResolveTableNameFromReflection(Type type)
    {
        var typeInfo = type.GetTypeInfo();
        var tableAttr = typeInfo.GetCustomAttribute<TableAttribute>();
        if (tableAttr != null)
        {
            if (!string.IsNullOrEmpty(tableAttr.Schema))
            {
                return $"{tableAttr.Schema}.{tableAttr.Name}";
            }

            return tableAttr.Name;
        }

        // Fall back to plural of table name
        var name = type.Name;
        if (name.EndsWith("y", StringComparison.OrdinalIgnoreCase))
        {
            // Category -> Categories
            name = name.Remove(name.Length - 1) + "ies";
        }
        else if (!name.EndsWith("s", StringComparison.OrdinalIgnoreCase))
        {
            // Product -> Products
            name += "s";
        }

        return name;
    }
}
