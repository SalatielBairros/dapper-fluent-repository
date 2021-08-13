using System.Linq.Expressions;
using System.Reflection;
using System;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Utils;

namespace Dapper.Fluent.ORM.Mapping
{
    public abstract class DapperFluentEntityMap<TEntity> : DommelEntityMap<TEntity>, IDapperFluentEntityMap where TEntity : class
    {
        public string Schema { get; }

        public DapperFluentEntityMap(string schema)
        {
            Schema = schema;
        }

        protected DommelPropertyMap MapToColumn(Expression<Func<TEntity, object>> expression)
        {
            var property = base.Map(expression);
            return property.ToColumn(property.ColumnName.ToLowerInvariant(), false);
        }
    }
}
