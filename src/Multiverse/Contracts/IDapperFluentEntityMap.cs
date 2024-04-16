using Dapper.FluentMap.Mapping;

namespace Multiverse.Mapping;

public interface IDapperFluentEntityMap : IEntityMap
{
    string TableName { get; }
    string Schema { get; }
    bool IsValidated { get; }
    bool IsDynamicSchema { get; }
    void WithSchema(string schema);
}
