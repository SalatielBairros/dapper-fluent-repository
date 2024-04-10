using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Dapper.Fluent.ORM.Migrations;
using Dapper.Fluent.ORM.Contracts;
using System.Reflection;
using System.Linq;
using Dommel;
using Dapper.Fluent.Mapping.Resolvers;
using Dapper.FluentMap;

namespace Dapper.Fluent.ORM.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection AddMigrator(this IServiceCollection services)
    {
        return services
            .AddFluentMigratorCore()
            .AddLogging(cfg => cfg.AddFluentMigratorConsole());
    }

    public static IMigrationRunnerBuilder ConfigureMigrator(this IMigrationRunnerBuilder @this, string connectionString, params Assembly[] assemblies)
    {
        return @this
            .WithGlobalConnectionString(connectionString)
            .ScanIn(assemblies.Append(typeof(DapperFluentMigration).Assembly).ToArray()).For.Migrations();
    }

    public static IServiceCollection AddMapperConfiguration<T>(this IServiceCollection services) where T : class, IMapperConfiguration
    {
        return services.AddFluentMapperConfiguration()
                .AddTransient<IMapperConfiguration, T>()
                .AddTransient<ISchema, GlobalSchemaProxy>();
    }

    public static IServiceCollection AddFluentMapperConfiguration(this IServiceCollection services)
    {
        services.AddTransient<ITableNameResolver, TableNameResolver>();

        FluentMapper.Initialize(config =>
        {
            DommelMapper.SetColumnNameResolver(new ColumnNameResolver());
            DommelMapper.SetKeyPropertyResolver(new KeyPropertyResolver());
            DommelMapper.SetPropertyResolver(new PropertyResolver());
        });

        return services;
    }

    public static IServiceCollection AddDapperORM(this IServiceCollection services)
        => services.AddTransient<IDapperORMRunner, DapperRepositoryRunner>();
}
