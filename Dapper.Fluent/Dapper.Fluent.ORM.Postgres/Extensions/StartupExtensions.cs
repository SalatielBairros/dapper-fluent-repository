using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Fluent.ORM.Extensions;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Fluent.ORM.Postgres.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddPostgresMigrator(this IServiceCollection services, string connectionString)
        {
            return services
                .AddMigrator()
                .ConfigureRunner(cfg => cfg
                    .ConfigureMigrator(connectionString)
                    .AddPostgres()
                );
        }
    }
}
