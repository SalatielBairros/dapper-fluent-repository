using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Repository;
using Npgsql;
using System.Data;

namespace Dapper.Fluent.ORM.Postgres;

public class PostgresDapperConnection : DapperConnection<NpgsqlConnection>
{
    private readonly IRepositorySettings _repositorySettings;

    public PostgresDapperConnection(IRepositorySettings repositorySettings) : base(repositorySettings)
    {
        _repositorySettings = repositorySettings;
    }

    public override IDbConnection GetConnection()
    {
        return new NpgsqlConnection
        {
            ConnectionString = _repositorySettings.ConnString
        };
    }
}