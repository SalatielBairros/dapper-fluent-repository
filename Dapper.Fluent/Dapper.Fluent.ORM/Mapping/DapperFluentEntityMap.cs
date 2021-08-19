using System.Linq.Expressions;
using System.Reflection;
using System;
using Dapper.Fluent.ORM.Extensions;
using Dapper.FluentMap.Mapping;

namespace Dapper.Fluent.ORM.Mapping
{
    public abstract class DapperFluentEntityMap<TEntity> : EntityMapBase<TEntity, DapperFluentPropertyMap>, IDapperFluentEntityMap where TEntity : class
    {
        public string Schema { get; private set; }
        public string TableName { get; private set; }
        public bool IsValidated { get; private set; } = false;
        public bool IsDynamicSchema { get; private set; } = false;

        public DapperFluentEntityMap(string schema)
        {
            Schema = schema;          
        }

        public DapperFluentEntityMap()
        {
            IsDynamicSchema = true;
        }

        protected override DapperFluentPropertyMap GetPropertyMap(PropertyInfo info) => new DapperFluentPropertyMap(info);

        protected DapperFluentPropertyMap MapToColumn(Expression<Func<TEntity, object>> expression)
        {
            var property = base.Map(expression);
            return property.ToColumn(property.ColumnName.ToLowerInvariant(), false);
        }

        protected new DapperFluentPropertyMap Map(Expression<Func<TEntity, object>> expression) => (DapperFluentPropertyMap)base.Map(expression);

        protected DapperFluentEntityMap<TEntity> WithEntityValidation()
        {
            IsValidated = true;
            return this;
        }

        public void WithSchema(string schema)
        {
            Schema = schema;
            ToTable(TableName.GetTableName(), Schema);
        }

        protected void ToTable(string tableName)
        {
            TableName = tableName;
        }

        protected void ToTable(string tableName, string schemaName)
        {
            TableName = $"{schemaName}.{tableName}";
        }
    }
}
