
using Dapper.Fluent.Application;
using Multiverse.Mapping;

namespace Dapper.Fluent.Repository.FluentMapper;

public class CategoryMap : DapperFluentEntityMap<Category>
{
    public CategoryMap(string schema)
        : base(schema)
    {
        ToTable("category", schema);
        MapToColumn(x => x.Id).IsKey().IsIdentity();
        MapToColumn(x => x.Description);
        MapToColumn(x => x.Data).AsJson();
    }
}
