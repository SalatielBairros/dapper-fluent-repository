using System.Collections.Generic;
using System;
using System.Linq;
using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Mapping;
using Dapper.FluentMap;
using FluentMigrator;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Fluent.ORM.Migrations;

[Migration(1, transactionBehavior: TransactionBehavior.None)]
public class DapperFluentMigration : OnlyUpMigration
{
    private readonly IServiceProvider _serviceProvider;


    public DapperFluentMigration(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override void Up()
    {
        var schemaProxy = _serviceProvider.GetService<ISchema>();
        var schema = schemaProxy.GetSchema();

        var maps = FluentMapper.EntityMaps.Values.Cast<IDapperFluentEntityMap>().ToList();
        CreateSchemas(maps, schema);
        maps.ForEach(map => this.CreateTableIfNotExists(map, schema));
    }

    private void CreateSchemas(IEnumerable<IDapperFluentEntityMap> maps, string tenantSchema)
    {
        var schemas = maps.Select(x => x.Schema).ToList();
        schemas.Add(tenantSchema);
        schemas.Distinct().ToList().ForEach(schema => this.CreateSchemaIfNotExists(schema));
    }
}
