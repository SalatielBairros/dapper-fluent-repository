using System;
using Dapper.Fluent.ORM.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.Fluent.Mapping.Resolvers
{
    public class TableNameResolver : ITableNameResolver
    {
        private static readonly ITableNameResolver DefaultResolver = new DefaultTableNameResolver();

        /// <inheritdoc />
        public string ResolveTableName(Type type)
        {
            IEntityMap entityMap;
            if (FluentMap.FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                var mapping = entityMap as IDapperFluentEntityMap;

                if (mapping != null)
                {
                    return mapping.TableName;
                }
            }

            return DefaultResolver.ResolveTableName(type);
        }
    }
}
