using System;
using System.Linq;
using Dapper.Fluent.ORM.Mapping;
using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.TypeMaps;
using static Dapper.SqlMapper;

namespace Dapper.Fluent.Mapping
{
    public static class FluentMapping
    {
        public static void AddMap<TEntity>(IEntityMap<TEntity> entityMap)
        {
            if (FluentMap.FluentMapper.EntityMaps.TryAdd(typeof(TEntity), entityMap))
            {
                SetTypeMap(typeof(TEntity), new FluentMapTypeMap<TEntity>());
            }
        }

        public static IDapperFluentEntityMap GetMapOf<T>()
        {
            FluentMap.FluentMapper.EntityMaps.TryGetValue(typeof(T), out var map);
            return (IDapperFluentEntityMap)map;
        }

        public static void SetDynamicSchema(string schema)
        {
            foreach (var map in FluentMap.FluentMapper.EntityMaps.Where(x => ((IDapperFluentEntityMap)x.Value).IsDynamicSchema))
            {
                ((IDapperFluentEntityMap)FluentMap.FluentMapper.EntityMaps[map.Key]).WithSchema(schema);

                Type[] typeArgs = { map.Key };
                var fluentMap = (ITypeMap)Activator.CreateInstance(typeof(FluentMapTypeMap<>).MakeGenericType(typeArgs));

                SetTypeMap(map.Key, fluentMap);
            }
        }
    }
}
