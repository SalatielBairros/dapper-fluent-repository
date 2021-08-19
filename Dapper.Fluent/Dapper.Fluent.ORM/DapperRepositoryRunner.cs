using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.MultiSchema;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Fluent.ORM
{
    public class DapperRepositoryRunner : IDapperORMRunner
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<string> _migratedSchemas;

        public DapperRepositoryRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _migratedSchemas = new List<string>();
        }

        public void AddMappers()
        {
            using var scope = _serviceProvider.CreateScope();
            scope.ServiceProvider
                .GetService<IMapperConfiguration>()
                .ConfigureMappers();            
        }

        public void CreateTablesFromMigrations()
        {
            using var scope = _serviceProvider.CreateScope();
            var requestInfo = scope.ServiceProvider.GetService<IRequestInfo>();

            if(requestInfo != null)
            {
                var schema = requestInfo.GetSchema();

                if (_migratedSchemas.Contains(schema))
                    return;

                scope.ServiceProvider
                    .GetService<IMapperConfiguration>()
                    .SetDynamicSchema(schema);

                _migratedSchemas.Add(schema);
            }

            scope.ServiceProvider
                .GetService<IMigrationRunner>()
                .MigrateUp();
        }
    }
}
