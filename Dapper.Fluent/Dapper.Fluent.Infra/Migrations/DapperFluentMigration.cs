using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Dapper.Fluent.Infra.Migrations
{
    [Migration(1)]
    public class DapperFluentMigration : Migration
    {
        public override void Up()
        {
            Create.Table("fluent_dapper_entity")
                .InSchema("public")
                .WithColumn("id").AsInt32().NotNullable().Identity().PrimaryKey("pk_fluent_dapper_entity")
                .WithColumn("intproperty").AsInt32().Nullable()
                .WithColumn("textproperty").AsString().Nullable()
                .WithColumn("limitedtextproperty").AsFixedLengthString(255).Nullable()
                .WithColumn("booleanproperty").AsBoolean().Nullable()
                .WithColumn("dateproperty").AsDateTime().Nullable()
                .WithColumn("decimalproperty").AsDecimal().Nullable();

            Create.Schema("dapper");            

            Create.Table("logentity")
                .InSchema("dapper")
                .WithColumn("id").AsInt32().NotNullable().Identity().PrimaryKey("pk_fluent_dapper_entity")
                .WithColumn("publicid").AsInt32().NotNullable()
                .WithColumn("intproperty").AsInt32().Nullable()
                .WithColumn("textproperty").AsString().Nullable()
                .WithColumn("limitedtextproperty").AsFixedLengthString(255).Nullable()
                .WithColumn("booleanproperty").AsBoolean().Nullable()
                .WithColumn("dateproperty").AsDateTime().Nullable()
                .WithColumn("decimalproperty").AsDecimal().Nullable();
        }

        public override void Down()
        {
            Delete.Table("fluent_dapper_entity");
            Delete.Table("dapper");
        }
    }
}
