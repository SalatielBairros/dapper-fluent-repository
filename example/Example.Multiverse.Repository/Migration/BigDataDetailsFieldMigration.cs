using Multiverse.Contracts;
using Multiverse.Migrations;
using Dapper.Fluent.Repository.FluentMapper;
using FluentMigrator;

namespace Dapper.Fluent.Repository.Migration;

[Migration(202404112, TransactionBehavior.None, "Details field")]
public class BigDataDetailsFieldMigration : OnlyUpMigration
{
    private readonly ISchema _schema;    

    public BigDataDetailsFieldMigration(ISchema schema)
    {
        this._schema = schema;
    }

    public override void Up()
    {
        var map = new BigDataMap();
        var tablename = map.TableName;
        var schemaName = _schema.GetSchema();
        const string columnName = "details";

        if (!Schema.Schema(schemaName).Table(tablename).Column(columnName).Exists())
            this.Alter.Table(tablename)
                .AddColumn(columnName)
                .AsString()
                .Nullable();
    }
}