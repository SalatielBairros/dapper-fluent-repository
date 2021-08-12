using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Dapper.Fluent.ORM.Migrations;

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

        public static IMigrationRunnerBuilder ConfigureMigrator(this IMigrationRunnerBuilder @this, string connectionString)
        {
            return @this
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DapperFluentMigration).Assembly).For.Migrations();
        }
    }
}
