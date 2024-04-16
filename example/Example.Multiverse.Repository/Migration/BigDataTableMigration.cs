using Multiverse.Contracts;
using Multiverse.Migrations;
using Dapper.Fluent.Repository.FluentMapper;
using FluentMigrator;

namespace Dapper.Fluent.Repository.Migration;

[Migration(202404111, TransactionBehavior.None, "Creating bigdata table")]
public class BigDataTableMigration : OnlyUpMigration
{
    private readonly ISchema _schema;

    public BigDataTableMigration(ISchema schema)
    {
        this._schema = schema;
    }

    public override void Up()
    {
        var map = new BigDataMap();
        this.CreateTableIfNotExists(map, _schema.GetSchema());
    }
}
