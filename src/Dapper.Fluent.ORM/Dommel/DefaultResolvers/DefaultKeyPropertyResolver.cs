using Dapper.Fluent.ORM.Mapping;
using Dapper.FluentMap.Mapping;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Dapper.Fluent.ORM.Dommel;

/// <summary>
/// Implements the <see cref="IKeyPropertyResolver"/> interface by resolving key properties
/// with the [<see cref="KeyAttribute"/>] or with the name 'Id'.
/// </summary>
public class DefaultKeyPropertyResolver : IKeyPropertyResolver
{
    /// <summary>
    /// Finds the key properties by looking for properties with the
    /// [<see cref="KeyAttribute"/>] attribute or with the name 'Id'.
    /// </summary>
    public ColumnPropertyInfo[] ResolveKeyProperties(Type type)
    {
        IEntityMap entityMap;
        if (!FluentMap.FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
        {
            return ResolveKeyPropertiesFromReflection(type);
        }

        var mapping = entityMap as IDapperFluentEntityMap;
        if (mapping != null)
        {
            var allPropertyMaps = entityMap.PropertyMaps.OfType<DapperFluentPropertyMap>();
            var keyPropertyInfos = allPropertyMaps
                 .Where(e => e.Key)
                 .Select(x => new ColumnPropertyInfo(x.PropertyInfo, x.GeneratedOption ?? (x.Identity ? DatabaseGeneratedOption.Identity : DatabaseGeneratedOption.None)))
                 .ToArray();

            try
            {
                var defaultKeyPropertyInfos = ResolveKeyPropertiesFromReflection(type)
                    .Where(x => allPropertyMaps.Count(y => y.PropertyInfo.Equals(x.Property)) == 0);
                keyPropertyInfos = keyPropertyInfos.Union(defaultKeyPropertyInfos).ToArray();
            }
            catch
            {
                if (keyPropertyInfos.Length == 0)
                {
                    throw new InvalidOperationException($"Could not find the key properties for type '{type.FullName}'.");
                }
            }

            return keyPropertyInfos;
        }

        return ResolveKeyPropertiesFromReflection(type);
    }

    private ColumnPropertyInfo[] ResolveKeyPropertiesFromReflection(Type type)
    {
        var keyProps = ORM.Dommel.Resolvers
            .Properties(type)
            .Select(x => x.Property)
            .Where(p => string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase) || p.GetCustomAttribute<KeyAttribute>() != null)
            .ToArray();

        if (keyProps.Length == 0)
        {
            throw new InvalidOperationException($"Could not find the key properties for type '{type.FullName}'.");
        }

        return keyProps.Select(p => new ColumnPropertyInfo(p, isKey: true)).ToArray();
    }
}
