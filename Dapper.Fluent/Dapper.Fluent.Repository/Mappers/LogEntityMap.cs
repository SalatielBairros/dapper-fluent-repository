
using Dapper.Fluent.Domain;
using Dapper.Fluent.ORM.Mapping;

namespace Dapper.Fluent.Repository.FluentMapper
{
    public class LogEntityMap : DapperFluentEntityMap<LogEntity>
    {
        public LogEntityMap()
            : base()
        {
            ToTable("log");
            MapToColumn(x => x.Id).IsKey().IsIdentity();
            MapToColumn(x => x.IntProperty).Default(5).NotNull();
            MapToColumn(x => x.LimitedTextProperty).WithLenght(255);
            MapToColumn(x => x.PublicId);
            MapToColumn(x => x.TextProperty);
            Map(x => x.DateProperty).ToColumn("date", false);
            MapToColumn(x => x.DecimalProperty);
            MapToColumn(x => x.BooleanProperty);
        }
    }
}
