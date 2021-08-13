using Dapper.FluentMap.Mapping;

namespace Dapper.Fluent.Mapping
{
    public static class FluentMapping
    {        
        public static void AddMap<TEntity>(IEntityMap<TEntity> entityMap)
        {
            FluentMap.FluentMapper.EntityMaps.TryAdd(typeof(TEntity), entityMap);
        }
    }
}
