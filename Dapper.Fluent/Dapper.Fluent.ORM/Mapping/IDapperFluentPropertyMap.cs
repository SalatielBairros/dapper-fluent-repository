using Dapper.FluentMap.Mapping;

namespace Dapper.Fluent.ORM.Mapping;

public interface IDapperFluentPropertyMap : IPropertyMap
{
    object DefaultValue { get; }
    int Lenght { get; }
}
