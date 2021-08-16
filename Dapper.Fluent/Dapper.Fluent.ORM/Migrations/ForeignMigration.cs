using System.Linq;
using Dapper.Fluent.ORM.Mapping;
using FluentMigrator;

namespace Dapper.Fluent.ORM.Migrations
{
    [Migration(2)]
    public class ForeignMigration : OnlyUpMigration
    {
        public override void Up()
        {
            FluentMap.FluentMapper.EntityMaps.Values.Cast<IDapperFluentEntityMap>().ToList()
                .ForEach(this.CreateForeignKey);
        }
    }
}
