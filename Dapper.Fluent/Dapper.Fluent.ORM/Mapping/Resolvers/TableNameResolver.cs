using System;
using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Mapping;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.Fluent.Mapping.Resolvers
{
    public class TableNameResolver : ITableNameResolver
    {
        private readonly ISchema _schema;
        private static readonly ITableNameResolver DefaultResolver = new DefaultTableNameResolver();

        public TableNameResolver(ISchema schema)
        {
            _schema = schema;
        }

        public string ResolveTableName(Type type)
        {
            if (!FluentMapper.EntityMaps.TryGetValue(type, out var entityMap))
                return DefaultResolver.ResolveTableName(type);

            if (!(entityMap is IDapperFluentEntityMap mapping))
                return DefaultResolver.ResolveTableName(type);

            var tableName = mapping.TableName;

            var schema = _schema.GetSchema();

            if (tableName.Contains(".") && mapping.IsDynamicSchema && !string.IsNullOrWhiteSpace(schema))
            {
                return $"{schema}.{tableName.Split(".")[1]}";
            }

            return tableName;

        }
    }
}
