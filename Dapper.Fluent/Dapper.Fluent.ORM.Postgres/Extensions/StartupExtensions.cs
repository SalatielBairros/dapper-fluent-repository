using System.Reflection;
using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Extensions;
using Dapper.Fluent.ORM.Postgres.Contracts;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Fluent.ORM.Postgres.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddPostgresRepositoryWithMigration(this IServiceCollection services, string connectionString, string defaultSchema = "public", params Assembly[] assembliesWithMappers)
        {
            services.AddScoped(typeof(IRepositorySettings), service => new PostgresRepositorySettings
            {
                ConnString = connectionString,
                DefaultSchema = defaultSchema ?? "public"
            });

            services.AddScoped(typeof(IPostgresRepository<>), typeof(PostgresRepository<>));

            return services
                .AddMigrator()
                .ConfigureRunner(cfg => cfg
                    .ConfigureMigrator(connectionString, assembliesWithMappers)
                    .AddPostgres()
                );
        }
    }
}
