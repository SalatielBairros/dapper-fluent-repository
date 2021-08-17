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
        public bool IsValidated { get; private set; } = false;

        public DapperFluentEntityMap(string schema)
        {
            Schema = schema;          
        }

        protected override DommelPropertyMap GetPropertyMap(PropertyInfo info) => new DapperFluentPropertyMap(info);

        protected DapperFluentPropertyMap MapToColumn(Expression<Func<TEntity, object>> expression)
        {
            var property = base.Map(expression);
            return (DapperFluentPropertyMap)property.ToColumn(property.ColumnName.ToLowerInvariant(), false);
        }

        protected new DapperFluentPropertyMap Map(Expression<Func<TEntity, object>> expression) => (DapperFluentPropertyMap)base.Map(expression);

        protected DapperFluentEntityMap<TEntity> WithEntityValidation()
        {
            IsValidated = true;
            return this;
        }
    }
}
