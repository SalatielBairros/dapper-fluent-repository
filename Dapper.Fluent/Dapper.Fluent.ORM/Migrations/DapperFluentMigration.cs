using System.Linq;
using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Mapping;
using FluentMigrator;

namespace Dapper.Fluent.ORM.Migrations
{
    [Migration(1)]
    public class DapperFluentMigration : OnlyUpMigration
    {
        private readonly IRepositorySettings _settings;

        public DapperFluentMigration(IRepositorySettings settings)
        {
            _settings = settings;
        }

        public override void Up()
        {
            this.CreateSchemaIfNotExists(_settings.Schema);

            foreach (var map in FluentMap.FluentMapper.EntityMaps.Values.Cast<IDapperFluentEntityMap>())
            {
                this.CreateTableIfNotExists(map, _settings.Schema);
            }
        }
    }
}
