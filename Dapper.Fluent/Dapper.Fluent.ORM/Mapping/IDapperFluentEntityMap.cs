using Dapper.FluentMap.Dommel.Mapping;

namespace Dapper.Fluent.ORM.Mapping
{
    public interface IDapperFluentEntityMap : IDommelEntityMap
    {
        string Schema { get; }
        bool IsValidated { get; }
        bool IsDynamicSchema { get; }
        void WithSchema(string schema);
    }
}
