using System.Linq;
using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Mapping;
using FluentMigrator;

namespace Dapper.Fluent.ORM.Migrations
{
    [Migration(1)]
    public class DapperFluentMigration : OnlyUpMigration
    {
        public override void Up()
        {
            var maps = FluentMap.FluentMapper.EntityMaps.Values.Cast<IDapperFluentEntityMap>().ToList();

            maps.Select(x => x.Schema).Distinct().ToList().ForEach(schema => this.CreateSchemaIfNotExists(schema));
            maps.ForEach(map => this.CreateTableIfNotExists(map));
        }
    }
}
