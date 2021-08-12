
using Dapper.Fluent.Domain;
using Dapper.Fluent.ORM.Mapping;

namespace Dapper.Fluent.Infra.FluentMapper
{
    public class PublicSchemaEntityMap : DapperFluentEntityMap<PublicSchemaEntity>
    {
        public PublicSchemaEntityMap()
        {
            ToTable("sampleentity");
            ToSchema("dapper");
            MapToColumn(x => x.Id).IsKey().IsIdentity();
            MapToColumn(x => x.IntProperty);
            MapToColumn(x => x.LimitedTextProperty);
            MapToColumn(x => x.TextProperty);
            Map(x => x.DateProperty).ToColumn("date");
            MapToColumn(x => x.DecimalProperty);
            MapToColumn(x => x.BooleanProperty);
        }
    }
}
