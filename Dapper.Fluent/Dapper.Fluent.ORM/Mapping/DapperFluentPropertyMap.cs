using System;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;

namespace Dapper.Fluent.ORM.Mapping
{
    public class DapperFluentPropertyMap : DommelPropertyMap
    {
        public DapperFluentPropertyMap(PropertyInfo info)
            : base(info)
        {
        }

        public object DefaultValue { get; private set; }
        public int Lenght { get; private set; }
        public ForeignKeyMap ForeignKey { get; private set; }
        public bool AllowNull { get; private set; } = true;

        public bool HasLenght() => Lenght > 0;
        public bool HasDefaultValue() => DefaultValue != null;
        public bool NotNull() => AllowNull = false;
        public bool IsForeignKey() => ForeignKey != null && ForeignKey.IsValid();

        public DapperFluentPropertyMap WithLenght(int lenght)
        {
            Lenght = lenght;
            return this;
        }

        public DapperFluentPropertyMap Default(object defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        public DapperFluentPropertyMap WithForeignKey(string primaryTable, string primaryColumn, string primarySchema)
        {
            var fkName = $"FK_{ColumnName}_{primaryColumn}_{primaryTable}";
            ForeignKey = new ForeignKeyMap(fkName, primaryTable, primaryColumn, primarySchema);
            return this;
        }

        public class ForeignKeyMap
        {
            public ForeignKeyMap(string fkName, string primaryTable, string primaryColumn, string primarySchema)
            {
                FKName = fkName;
                PrimaryTable = primaryTable;
                PrimaryColumn = primaryColumn;
                PrimarySchema = primarySchema;
            }

            public string FKName { get; }
            public string PrimaryTable { get; }
            public string PrimaryColumn { get; }
            public string PrimarySchema { get; }

            public bool IsValid() 
                => !string.IsNullOrWhiteSpace(PrimaryTable)
                && !string.IsNullOrWhiteSpace(FKName)
                && !string.IsNullOrWhiteSpace(PrimaryColumn)
                && !string.IsNullOrWhiteSpace(PrimarySchema);
        }
    }
}
