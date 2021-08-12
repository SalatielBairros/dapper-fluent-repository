using Dapper.Fluent.Infra.Contracts;
using Dapper.Fluent.Migrations.Extensions;
using Dapper.Fluent.Migrations.Migrations;
using FluentMigrator;

namespace Dapper.Fluent.Infra.Migrations
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
            this.CreateTableIfNotExists<SampleEntity>(_configuration.Schema);
            this.CreateTableIfNotExists<LogEntity>(_configuration.Schema);            
        }
    }
}
