using Multiverse.Mapping;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap;

namespace Multiverse.Dommel;

/// <summary>
/// Implements the <see cref="IKeyPropertyResolver"/>.
/// </summary>
public class DefaultColumnNameResolver : IColumnNameResolver
{
    /// <summary>
    /// Resolves the column name for the property.
    /// Looks for the [Column] attribute. Otherwise it's just the name of the property.
    /// </summary>
    public string ResolveColumnName(PropertyInfo propertyInfo)
    {
        if (propertyInfo.DeclaringType != null)
        {
            if (FluentMapper.EntityMaps.TryGetValue(propertyInfo.ReflectedType, out var entityMap))
            {
                var mapping = entityMap as IDapperFluentEntityMap;
                if (mapping != null)
                {
                    var propertyMaps = entityMap.PropertyMaps.Where(m => m.PropertyInfo.Name == propertyInfo.Name).ToList();
                    if (propertyMaps.Count == 1)
                    {
                        return propertyMaps[0].ColumnName;
                    }
                }
            }
            else if (FluentMapper.TypeConventions.TryGetValue(propertyInfo.ReflectedType, out var conventions))
            {
                foreach (var convention in conventions)
                {
                    var propertyMaps = convention.PropertyMaps.Where(m => m.PropertyInfo.Name == propertyInfo.Name).ToList();
                    if (propertyMaps.Count == 1)
                    {
                        return propertyMaps[0].ColumnName;
                    }
                }
            }
        }

        var columnAttr = propertyInfo.GetCustomAttribute<ColumnAttribute>();
        return columnAttr?.Name ?? propertyInfo.Name;
    }
}
