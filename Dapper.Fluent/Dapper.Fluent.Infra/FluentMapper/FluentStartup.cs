using Dapper.FluentMap.Dommel;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Fluent.Infra.FluentMapper
{
    public static class FluentStartup
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            FluentMap.FluentMapper.Initialize(config =>
            {
                config.AddMap(new LogEntityMap());
                config.AddMap(new PublicSchemaEntityMap());
                config.ForDommel();
            });

            return services;
        }
    }
}
