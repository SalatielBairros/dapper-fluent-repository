using Dapper.FluentMap.Dommel.Mapping;

namespace Dapper.Fluent.ORM.Mapping
{
    public interface IDapperFluentEntityMap : IDommelEntityMap
    {
        bool PublicSchema { get; }
    }
}
