using System;
using System.Collections.Generic;
using System.Linq;
using Dapper.Fluent.ORM.Mapping;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using static Dapper.SqlMapper;

namespace Dapper.Fluent.Mapping;

public static class FluentMapping
{
    public static void AddMap<TEntity>(IEntityMap<TEntity> entityMap)
    {
        if (FluentMapper.EntityMaps.TryAdd(typeof(TEntity), entityMap))
        {
            SetTypeMap(typeof(TEntity), new DommelFluentMapTypeMap<TEntity>());
        }
    }

    public static IDapperFluentEntityMap GetMapOf<T>()
    {
        FluentMapper.EntityMaps.TryGetValue(typeof(T), out var map);
        return (IDapperFluentEntityMap)map;
    }

    public static void SetDynamicSchema(string schema)
    {
        foreach (var map in FluentMapper.EntityMaps.Where(x => ((IDapperFluentEntityMap)x.Value).IsDynamicSchema))
        {
            ((IDapperFluentEntityMap)FluentMapper.EntityMaps[map.Key]).WithSchema(schema);

            Type[] typeArgs = { map.Key };
            var fluentMap = (ITypeMap)Activator.CreateInstance(typeof(DommelFluentMapTypeMap<>).MakeGenericType(typeArgs));

            SetTypeMap(map.Key, fluentMap);
        }
    }

    public static IEnumerable<Type> GetJsonTypes()
    {
        return FluentMapper.EntityMaps
            .SelectMany(x => x.Value.PropertyMaps)
            .Cast<DapperFluentPropertyMap>()
            .Where(x => x.IsJson)
            .Select(x => x.PropertyInfo.PropertyType)
            .Distinct();
    }
}
