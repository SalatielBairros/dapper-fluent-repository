using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator;
using IFluentSyntax = FluentMigrator.Infrastructure.IFluentSyntax;
using System.Reflection;
using Dapper.Fluent.Repository.Attributes;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;

namespace Dapper.Fluent.Migrations.Extensions
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

        public static IFluentSyntax CreateTableIfNotExists(this MigrationBase self, string schema, string tableName, Func<ICreateTableWithColumnSyntax, IFluentSyntax> creationFunc)
        {
            if (!self.Schema.Schema(schema).Table(tableName).Exists())
            {
                return creationFunc(self.Create.Table(tableName).InSchema(schema));
            }
            else
            {
                return null;
            }
        }

        public static IFluentSyntax CreateTableIfNotExists<TEntity>(this MigrationBase self, string schema)
        {
            var tableName = GetTableName<TEntity>();
            if (!self.Schema.Schema(schema).Table(tableName).Exists())
            {
                return self.Create.Table(tableName).InSchema(schema).AddColumns<TEntity>();
            }
            else
            {
                return null;
            }
        }

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

        public static ICreateTableColumnOptionOrWithColumnSyntax As(this ICreateTableColumnAsTypeSyntax column, Type type) => mappedTypes[type](column);

        private static ICreateTableWithColumnSyntax AddColumns<TEntity>(this ICreateTableWithColumnSyntax table)
        {
            foreach (var column in GetColumns<TEntity>())
            {
                var c = table.WithColumn(column.Name).As(column.Type);
                if (column.Info.IsIdentity)
                    c.Identity();
                if (column.Info.IsPK)
                    c.PrimaryKey();
            }
            return table;
        }

        private static List<(string Name, Type Type, (bool IsPK, bool IsIdentity) Info)> GetColumns<TEntity>()
        {
            return typeof(TEntity).GetProperties()
                .Where(p => !((p.PropertyType.IsClass && p.PropertyType.Name != "String") || p.PropertyType.IsInterface))
                .Select(a => (GetColumnName(a), a.PropertyType, GetColumnInfo(a)))
                .ToList();
        }

        private static string GetTableName<TEntity>()
        {
            return typeof(TEntity).GetCustomAttribute<TableAttribute>()?.TableName?.Replace(".", "_") ?? typeof(TEntity).Name.ToLowerInvariant();
        }

        private static string GetColumnName(PropertyInfo p)
        {
            return p.GetCustomAttribute<ColumnAttribute>()?.ColumnName?.Replace(".", "_") ?? p.Name.ToLowerInvariant();
        }

        private static (bool IsPK, bool IsIdentity) GetColumnInfo(PropertyInfo p)
        {
            bool isPK = p.GetCustomAttribute<PrimaryKey>() != null;
            var isIdentity = p.GetCustomAttribute<Identity>() != null;

            return (isPK, isIdentity);
        }        
    }

    public static class StartupExtensions
    {
        public static IServiceCollection AddMigrator(this IServiceCollection services, string connectionString, Assembly migrationsAssembly)
        {
            services
                .AddFluentMigratorCore()
                .ConfigureRunner(cfg => cfg
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(migrationsAssembly).For.Migrations()
                )
                .AddLogging(cfg => cfg.AddFluentMigratorConsole());

            return services;
        }
    }
}
