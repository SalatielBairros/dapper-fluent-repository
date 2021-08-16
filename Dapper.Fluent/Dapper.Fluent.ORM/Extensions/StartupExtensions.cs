using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Dapper.Fluent.ORM.Migrations;
using Dapper.FluentMap.Dommel;
using Dapper.Fluent.ORM.Contracts;
using System.Reflection;
using System.Linq;

namespace Dapper.Fluent.ORM.Extensions
{
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
            FluentMap.FluentMapper.Initialize(config =>
            {
                config.ForDommel();
            });
            return services.AddScoped<IMapperConfiguration, T>();
        }

        public static IServiceCollection AddDapperORM(this IServiceCollection services)
        {
            return services.AddScoped<IDapperORMRunner, DapperRepositoryRunner>();
        }

    }
}
