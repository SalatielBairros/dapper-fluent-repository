using Dapper.FluentMap.Dommel.Mapping;

namespace Dapper.Fluent.Mapping.FluentMapper
{
    public interface IDapperFluentEntityMap : IDommelEntityMap
    {
        bool PublicSchema { get; }
    }
}
