using System.Linq.Expressions;
using System.Reflection;
using System;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Utils;

namespace Dapper.Fluent.Mapping.FluentMapper
{
    public abstract class DapperFluentEntityMap<TEntity> : DommelEntityMap<TEntity>, IDapperFluentEntityMap where TEntity : class
    {
        public bool PublicSchema { get; private set; } = false;

        public void IsPublicSchema()
        {
            PublicSchema = true;
        }

        public void IsPrivateSchema()
        {
            PublicSchema = false;
        }

        protected DommelPropertyMap MapToColumn(Expression<Func<TEntity, object>> expression)
        {
            var property = base.Map(expression);
            return property.ToColumn(property.ColumnName.ToLowerInvariant(), false);
        }
    }
}
