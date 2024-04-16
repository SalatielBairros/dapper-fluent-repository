using System.Reflection;
using Multiverse.Contracts;
using Multiverse.Extensions;
using Multiverse.Postgres.Contracts;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Multiverse.Postgres.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection AddPostgresRepositoryWithMigration(this IServiceCollection services, string connectionString, string defaultSchema = "public", params Assembly[] assembliesWithMappers)
    {
        services.AddScoped(typeof(IRepositorySettings), service => new PostgresRepositorySettings
        {
            ConnString = connectionString,
            DefaultSchema = defaultSchema ?? "public"            
        });

        services
            .AddTransient(typeof(IPostgresRepository<>), typeof(PostgresRepository<>))
            .AddScoped<IJsonPropertyHandler, PostgresJsonPropertyHandler>();

        return services
            .AddMigrator()
            .ConfigureRunner(cfg => cfg
                .ConfigureMigrator(connectionString, assembliesWithMappers)
                .AddPostgres()
            );
    }
}
