using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.TypeMaps;

namespace Dapper.Fluent.Mapping
{
    public static class FluentMapping
    {
        public static void AddMap<TEntity>(IEntityMap<TEntity> entityMap)
        {
            if (FluentMap.FluentMapper.EntityMaps.TryAdd(typeof(TEntity), entityMap))
            {
                SqlMapper.SetTypeMap(typeof(TEntity), new FluentMapTypeMap<TEntity>());
            }
        }
    }
}
