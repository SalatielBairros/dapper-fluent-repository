using System.Linq;
using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Mapping;
using FluentMigrator;

namespace Dapper.Fluent.ORM.Migrations
{
    [Migration(1)]
    public class DapperFluentMigration : OnlyUpMigration
    {
        private readonly ISchemaConfiguration _configuration;

        public DapperFluentMigration(ISchemaConfiguration schemaConfiguration)
        {
            _configuration = schemaConfiguration;
        }

        public override void Up()
        {
            this.CreateSchemaIfNotExists(_configuration.Schema);

            foreach (var map in FluentMap.FluentMapper.EntityMaps.Values.Cast<IDapperFluentEntityMap>())
            {
                this.CreateTableIfNotExists(map, _configuration.Schema);
            }
        }
    }
}
