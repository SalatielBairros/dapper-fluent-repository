
using Dapper.Fluent.Application;
using Multiverse.Mapping;
using FluentMigrator;

namespace Dapper.Fluent.Repository.FluentMapper;

public class UsePeakMap : DapperFluentEntityMap<UsePeak>
{
    public UsePeakMap()
        : base()
    {
        ToTable("usepeak");
        WithEntityValidation();
        MapToColumn(x => x.Id).IsKey();
        MapToColumn(x => x.TCode).NotNull();
        MapToColumn(x => x.TenantId).NotNull();
        MapToColumn(x => x.PeakDate).NotNull();
        MapToColumn(x => x.SlotId).NotNull();
        MapToColumn(x => x.EventProcessedDate).NotNull();
        MapToColumn(x => x.UsePeakTotal).NotNull();
        MapToColumn(x => x.TotalLicenses).NotNull();
        MapToColumn(x => x.UsedAllLicenses).NotNull();
        MapToColumn(x => x.CreatedTime).Ignore().Default(SystemMethods.CurrentDateTime).NotNull();
    }
}