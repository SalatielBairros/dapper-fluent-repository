using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Multiverse.Migrations;
using Multiverse.Contracts;
using System.Reflection;
using System.Linq;
using Multiverse.Dommel;
using Dapper.FluentMap;

namespace Multiverse.Extensions;

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
        return services
                .AddTransient<ITableNameResolver, DefaultTableNameResolver>()
                .AddTransient<IMapperConfiguration, T>()
                .AddTransient<ISchema, GlobalSchemaProxy>();
    }

    public static IServiceCollection AddDapperORM(this IServiceCollection services)
        => services.AddTransient<IDapperORMRunner, DapperRepositoryRunner>();
}
