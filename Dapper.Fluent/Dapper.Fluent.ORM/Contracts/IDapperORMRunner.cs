
namespace Dapper.Fluent.ORM.Contracts
{
    public interface IDapperORMRunner
    {
        void AddMappers();
        void CreateTablesFromMigrations();
    }
}
