using Dapper.FluentMap.Mapping;

namespace Multiverse.Mapping;

public interface IDapperFluentPropertyMap : IPropertyMap
{
    object DefaultValue { get; }
    int Lenght { get; }
}
