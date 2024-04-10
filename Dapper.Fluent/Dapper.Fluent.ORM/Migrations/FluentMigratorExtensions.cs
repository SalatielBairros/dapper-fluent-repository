using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator;
using IFluentSyntax = FluentMigrator.Infrastructure.IFluentSyntax;
using Dapper.FluentMap.Mapping;
using Dapper.Fluent.ORM.Mapping;
using Dapper.Fluent.ORM.Extensions;

namespace Dapper.Fluent.ORM.Migrations
{
    public static class FluentMigratorExtensions
    {
        private readonly static Dictionary<Type, Func<ICreateTableColumnAsTypeSyntax, ICreateTableColumnOptionOrWithColumnSyntax>> mappedTypes = new Dictionary<Type, Func<ICreateTableColumnAsTypeSyntax, ICreateTableColumnOptionOrWithColumnSyntax>>
        {
            [typeof(int)] = c => c.AsInt32(),
            [typeof(int?)] = c => c.AsInt32(),
            [typeof(Int64)] = c => c.AsInt64(),
            [typeof(short)] = c => c.AsInt16(),
            [typeof(short?)] = c => c.AsInt16(),
            [typeof(long)] = c => c.AsInt64(),
            [typeof(long?)] = c => c.AsInt64(),
            [typeof(float)] = c => c.AsFloat(),
            [typeof(float?)] = c => c.AsFloat(),
            [typeof(double)] = c => c.AsDouble(),
            [typeof(double?)] = c => c.AsDouble(),
            [typeof(decimal)] = c => c.AsDecimal(),
            [typeof(decimal?)] = c => c.AsDecimal(),
            [typeof(string)] = c => c.AsString(),
            [typeof(String)] = c => c.AsString(),
            [typeof(bool)] = c => c.AsBoolean(),
            [typeof(bool?)] = c => c.AsBoolean(),
            [typeof(Dictionary<string, string>)] = c => c.AsCustom("hstore"),
            [typeof(Guid)] = c => c.AsGuid(),
            [typeof(Guid?)] = c => c.AsGuid(),
            [typeof(DateTime)] = c => c.AsDateTime2(),
            [typeof(DateTime?)] = c => c.AsDateTime2(),
            [typeof(TimeSpan)] = c => c.AsTime(),
            [typeof(byte[])] = c => c.AsCustom("bytea"),
            [typeof(string[])] = c => c.AsCustom("text[]"),
            [typeof(String[])] = c => c.AsCustom("text[]"),
            [typeof(Enum)] = c => c.AsInt32()
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

        public static IFluentSyntax CreateTableIfNotExists(this MigrationBase @this, IDapperFluentEntityMap map, string tenantchema)
        {
            var tableName = map.TableName.GetTableName();
            var schemaName = map.IsDynamicSchema && !string.IsNullOrEmpty(tenantchema) ? tenantchema : map.Schema;

            if (!@this.Schema.Schema(schemaName).Table(tableName).Exists())
            {
                return @this.Create.Table(tableName).InSchema(schemaName).AddColumns(map.PropertyMaps);
            }
            else
            {
                return null;
            }
        }

        public static void CreateForeignKey(this MigrationBase @this, IDapperFluentEntityMap entity)
        {
            entity.PropertyMaps.Cast<DapperFluentPropertyMap>()
                        .Where(p => p.IsForeignKey() && !@this.Schema.Schema(entity.Schema).Table(entity.TableName.GetTableName()).Constraint(p.ForeignKey.FKName).Exists())
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
                return @this.AsCustom($"varchar({column.Lenght})");
            }
            if (type.IsEnum || type.IsNullableEnum())
            {
                return @this.AsInt32();
            }
            if (!type.IsPrimitive && column.IsJson)
            {
                return @this.AsCustom("jsonb");
            }

            return mappedTypes[column.PropertyInfo.PropertyType](@this);
        }

        private static bool IsNullableEnum(this Type @this)
            => @this.IsGenericType && @this.GetGenericTypeDefinition() == typeof(Nullable<>) && @this.GetGenericArguments()[0].IsEnum;

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
                {
                    if (column.DefaultValue is SystemMethods)
                        c.WithDefault((SystemMethods)column.DefaultValue);
                    else
                        c.WithDefaultValue(column.DefaultValue);
                }
                if (column.AllowNull)
                    c.Nullable();
                else
                    c.NotNullable();
            }
            return table;
        }
    }
}
