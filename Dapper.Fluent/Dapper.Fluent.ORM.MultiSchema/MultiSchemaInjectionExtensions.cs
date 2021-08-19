using Dapper.Fluent.ORM.Migrations;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Fluent.ORM.MultiSchema
{
    public static class MultiSchemaInjectionExtensions
    {
        public static IServiceCollection AddDapperMultiSchemaOptions(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IRequestInfo, RequestInfo>();
            services.AddScoped<IVersionTableMetaData, MultiSchemaMigrationTable>();
            return services;
        }
    }
}
