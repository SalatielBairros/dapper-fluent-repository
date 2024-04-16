using System.Linq;
using Multiverse.Mapping;
using FluentMigrator;
using Dapper.FluentMap;

namespace Multiverse.Migrations
{
    [Migration(2, transactionBehavior: TransactionBehavior.None)]
    public class ForeignMigration : OnlyUpMigration
    {
        public override void Up()
        {
            FluentMapper.EntityMaps.Values.Cast<IDapperFluentEntityMap>().ToList()
                .ForEach(this.CreateForeignKey);
        }
    }
}
