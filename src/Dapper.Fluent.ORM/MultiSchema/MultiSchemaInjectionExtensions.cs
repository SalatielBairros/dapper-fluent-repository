using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Migrations;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Fluent.ORM.MultiSchema;

public static class MultiSchemaInjectionExtensions
{
    public static IServiceCollection AddHttpMultiSchema(this IServiceCollection services)
    {
        return services
            .AddHttpContextAccessor()
            .AddScoped<ISchema, HttpSchemaProxy>()
            .AddScoped<IVersionTableMetaData, MultiSchemaMigrationTable>();
    }

    public static IServiceCollection AddCustomMultiSchema<TSchemaResolver>(this IServiceCollection services)
        where TSchemaResolver : ISchema
    {
        return services
            .AddScoped<ISchema, HttpSchemaProxy>()
            .AddScoped<IVersionTableMetaData, MultiSchemaMigrationTable>();
    }
}
