
namespace Multiverse.Contracts;

public interface IDapperORMRunner
{
    void AddMappers();
    void CreateTablesFromMigrations();
}
