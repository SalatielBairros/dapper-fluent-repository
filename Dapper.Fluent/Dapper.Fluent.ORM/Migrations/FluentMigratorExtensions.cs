using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator;
using IFluentSyntax = FluentMigrator.Infrastructure.IFluentSyntax;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using Dapper.Fluent.ORM.Mapping;
using static Dapper.SqlMapper;

namespace Dapper.Fluent.ORM.Migrations
{
    public static class FluentMigratorExtensions
    {
        private readonly static Dictionary<Type, Func<ICreateTableColumnAsTypeSyntax, ICreateTableColumnOptionOrWithColumnSyntax>> mappedTypes = new Dictionary<Type, Func<ICreateTableColumnAsTypeSyntax, ICreateTableColumnOptionOrWithColumnSyntax>>
        {
            [typeof(int)] = c => c.AsInt32(),
            [typeof(Int64)] = c => c.AsInt64(),
            [typeof(short)] = c => c.AsInt16(),
            [typeof(long)] = c => c.AsInt64(),
            [typeof(float)] = c => c.AsFloat(),
            [typeof(double)] = c => c.AsDouble(),
            [typeof(decimal)] = c => c.AsDecimal(),
            [typeof(string)] = c => c.AsString(),
            [typeof(String)] = c => c.AsString(),
            [typeof(bool)] = c => c.AsBoolean(),
            [typeof(Dictionary<string, string>)] = c => c.AsCustom("hstore"),
            [typeof(Guid)] = c => c.AsGuid(),
            [typeof(DateTime)] = c => c.AsDateTime2(),
            [typeof(TimeSpan)] = c => c.AsTime(),
            [typeof(byte[])] = c => c.AsCustom("bytea"),
            [typeof(string[])] = c => c.AsCustom("text[]"),
            [typeof(String[])] = c => c.AsCustom("text[]"),
            [typeof(Enum)] = c => c.AsInt32(),
        };

        public static IFluentSyntax CreateSchemaIfNotExists(this MigrationBase self, string schema)
        {
            if (!self.Schema.Schema(schema).Exists())
            {
                return self.Create.Schema(schema);
            }
            else
            {
                return null;
            }
        }

        public static IFluentSyntax CreateTableIfNotExists(this MigrationBase @this, IDapperFluentEntityMap map)
        {
            var tableName = GetTableName(map.TableName);
            if (!@this.Schema.Schema(map.Schema).Table(tableName).Exists())
            {
                return @this.Create.Table(tableName).InSchema(map.Schema).AddColumns(map.PropertyMaps);
            }
            else
            {
                return null;
            }
        }

        public static void CreateForeignKey(this MigrationBase @this, IDapperFluentEntityMap entity)
        {
            entity.PropertyMaps.Cast<DapperFluentPropertyMap>()
                        .Where(p => p.IsForeignKey())
                        .ToList()
                        .ForEach(column =>
                        {
                            @this.Create
                                .ForeignKey(column.ForeignKey.FKName)
                                .FromTable(entity.TableName.GetTableName())
                                .ForeignColumns(column.ColumnName)
                                .ToTable(column.ForeignKey.PrimaryTable)
                                .PrimaryColumn(column.ForeignKey.PrimaryColumn);
                        });
        }

        private static ICreateTableColumnOptionOrWithColumnSyntax As(this ICreateTableColumnAsTypeSyntax @this, DapperFluentPropertyMap column)
        {
            var type = column.PropertyInfo.PropertyType;
            if (column.HasLenght() && type == typeof(string))
            {
                return @this.AsFixedLengthString(column.Lenght);
            }
            return mappedTypes[column.PropertyInfo.PropertyType](@this);
        }

        private static ICreateTableWithColumnSyntax AddColumns(this ICreateTableWithColumnSyntax table, IList<IPropertyMap> columns)
        {
            foreach (var column in columns.Cast<DapperFluentPropertyMap>())
            {
                var c = table.WithColumn(column.ColumnName).As(column);
                if (column.Identity)
                    c.Identity();
                if (column.Key)
                    c.PrimaryKey();
                if (column.HasDefaultValue())
                    c.WithDefaultValue(column.DefaultValue);
                if (column.AllowNull)
                    c.Nullable();
                else
                    c.NotNullable();
            }
            return table;
        }

        private static string GetTableName(this string fullTableName)
        {
            if (fullTableName.Contains('.'))
            {
                return fullTableName.Split('.')[1].ToLowerInvariant();
            }
            return fullTableName;
        }
    }
}
