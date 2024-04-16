using System;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.TypeMaps;

namespace Dapper.Fluent.Mapping;

public class DommelFluentMapTypeMap<TEntity> : MultiTypeMap
{
    public DommelFluentMapTypeMap()
        : base(new CustomPropertyTypeMap(typeof(TEntity), GetPropertyInfo), new DefaultTypeMap(typeof(TEntity)))
    {
    }

    private static PropertyInfo GetPropertyInfo(Type type, string columnName)
    {
        var cacheKey = $"{type.FullName};{columnName}";

        if (TypePropertyMapCache.TryGetValue(cacheKey, out PropertyInfo info))
        {
            return info;
        }

        if (FluentMapper.EntityMaps.TryGetValue(type, out IEntityMap entityMap))
        {
            var propertyMaps = entityMap.PropertyMaps;
            var propertyMap = propertyMaps.FirstOrDefault(m => MatchColumnNames(m, columnName));

            if (propertyMap != null)
            {
                TypePropertyMapCache.TryAdd(cacheKey, propertyMap.PropertyInfo);
                return propertyMap.PropertyInfo;
            }
        }

        TypePropertyMapCache.TryAdd(cacheKey, null);
        return null;
    }

    private static bool MatchColumnNames(IPropertyMap map, string columnName)
    {
        var comparison = StringComparison.Ordinal;
        if (!map.CaseSensitive)
        {
            comparison = StringComparison.OrdinalIgnoreCase;
        }

        return string.Equals(map.ColumnName, columnName, comparison);
    }
}
