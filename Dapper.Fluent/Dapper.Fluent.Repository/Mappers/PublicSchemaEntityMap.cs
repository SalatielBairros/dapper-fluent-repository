
using Dapper.Fluent.Domain;
using Dapper.Fluent.ORM.Mapping;

namespace Dapper.Fluent.Repository.FluentMapper
{
    public class PublicSchemaEntityMap : DapperFluentEntityMap<PublicSchemaEntity>
    {
        public PublicSchemaEntityMap(string schema)
            : base(schema)
        {
            ToTable("sampleentity", schema);
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
