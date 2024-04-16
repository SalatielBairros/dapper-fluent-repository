
using Dapper.Fluent.Application;
using Multiverse.Mapping;
using FluentMigrator;

namespace Dapper.Fluent.Repository.FluentMapper;

public class BigDataMap : DapperFluentEntityMap<BigData>
{
    public BigDataMap()
        : base()
    {
        ToTable("bigdata");
        WithEntityValidation();
        MapToColumn(x => x.Id).IsKey().IsIdentity();
        MapToColumn(x => x.Description).NotNull();
        MapToColumn(x => x.NumberValue).NotNull().Default(0);
        MapToColumn(x => x.CreationDate).Ignore().Default(SystemMethods.CurrentDateTime).NotNull();
        MapToColumn(x => x.Details);
    }
}