using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Dapper.Fluent.ORM.Migrations;

namespace Dapper.Fluent.ORM.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddMigrator(this IServiceCollection services, string connectionString)
        {
            services
                .AddFluentMigratorCore()
                .ConfigureRunner(cfg => cfg
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(DapperFluentMigration).Assembly).For.Migrations()
                )
                .AddLogging(cfg => cfg.AddFluentMigratorConsole());

            return services;
        }
    }
}
