using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Fluent.ORM.MultiSchema
{
    public static class MultiSchemaInjectionExtensions
    {
        public static IServiceCollection AddDapperMultiSchemaOptions(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IRequestInfo, RequestInfo>();
            return services;
        }
    }
}
