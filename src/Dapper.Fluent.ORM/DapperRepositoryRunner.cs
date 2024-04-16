using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Dapper.Fluent.Mapping;
using Dapper.Fluent.ORM.Contracts;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Fluent.ORM;

public class DapperRepositoryRunner : IDapperORMRunner
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly object ThisLock = new object();
    public static ConcurrentBag<string> MigratedSchemas = new ConcurrentBag<string>();

    public DapperRepositoryRunner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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
        lock (ThisLock)
        {
            using var scope = _serviceProvider.CreateScope();                        
            var schemaProxy = scope.ServiceProvider.GetService<ISchema>();

            var schema = schemaProxy.GetSchema();

            if (schema != null)
            {
                FluentMapping.SetDynamicSchema(schema);

                if (MigratedSchemas.Contains(schema))
                    return;

                var jsonProperties = FluentMapping.GetJsonTypes();
                if (jsonProperties.Any())
                {
                    var jsonHandler = scope.ServiceProvider.GetService<IJsonPropertyHandler>();
                    if (jsonHandler != null)
                    {
                        try
                        {
                            jsonHandler.SetJsonTypes(jsonProperties.ToArray());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            throw;
                        }
                    }
                }

                MigratedSchemas.Add(schema);
            }

            try
            {
                scope.ServiceProvider
                    .GetService<IMigrationRunner>()
                    .MigrateUp();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
