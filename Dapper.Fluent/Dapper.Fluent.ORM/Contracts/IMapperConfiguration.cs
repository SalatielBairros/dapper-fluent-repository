namespace Dapper.Fluent.ORM.Contracts
{
    public interface IMapperConfiguration
    {
        void ConfigureMappers();
        void SetDynamicSchema(string schema);
    }
}
