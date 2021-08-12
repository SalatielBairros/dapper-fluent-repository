
namespace Dapper.Fluent.ORM.Contracts
{
    public interface IRepositorySettings
    {
        string ConnString { get; set; }
        string Schema { get; set; }
    }
}
