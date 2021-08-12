
using Dapper.Fluent.Domain;
using Dapper.Fluent.ORM.Mapping;

namespace Dapper.Fluent.Infra.FluentMapper
{
    public class LogEntityMap : DapperFluentEntityMap<LogEntity>
    {
        public LogEntityMap()
        {
            ToTable("log", "dapper");            
            MapToColumn(x => x.Id).IsKey().IsIdentity();
            MapToColumn(x => x.IntProperty);
            MapToColumn(x => x.LimitedTextProperty);
            MapToColumn(x => x.PublicId);
            MapToColumn(x => x.TextProperty);
            Map(x => x.DateProperty).ToColumn("date", false);
            MapToColumn(x => x.DecimalProperty);
            MapToColumn(x => x.BooleanProperty);
        }
    }
}
