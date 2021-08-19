using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Dapper.Fluent.Mapping;
using Dapper.Fluent.ORM.Extensions;
using Dapper.FluentMap.Mapping;

namespace Dapper.Fluent.ORM.Mapping
{
    public class DapperFluentPropertyMap : PropertyMapBase<DapperFluentPropertyMap>, IPropertyMap
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

        public DapperFluentPropertyMap ForeignKeyFor(string primaryTable, string primaryColumn, string primarySchema)
        {
            var fkName = $"FK_{ColumnName}_{primaryColumn}_{primaryTable}";
            ForeignKey = new ForeignKeyMap(fkName, primaryTable, primaryColumn, primarySchema);
            return this;
        }

        public DapperFluentPropertyMap ForeignKeyFor<T>(string primaryColumn) where T : class
        {
            var map = FluentMapping.GetMapOf<T>();
            var column = map.PropertyMaps.Where(x => x.ColumnName == primaryColumn);
            var primaryTable = map.TableName.GetTableName();

            var fkName = $"FK_{ColumnName}_{primaryColumn}_{primaryTable}";
            ForeignKey = new ForeignKeyMap(fkName, primaryTable, primaryColumn, map.Schema);
            return this;
        }

        public bool Key { get; private set; }

        public bool Identity { get; set; }

        public DatabaseGeneratedOption? GeneratedOption { get; set; }

        public DapperFluentPropertyMap IsKey()
        {
            Key = true;
            return this;
        }

        public DapperFluentPropertyMap IsIdentity()
        {
            Identity = true;
            return this;
        }

        public DapperFluentPropertyMap SetGeneratedOption(DatabaseGeneratedOption option)
        {
            GeneratedOption = option;
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
