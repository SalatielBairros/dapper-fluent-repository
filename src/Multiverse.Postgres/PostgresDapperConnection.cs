using Multiverse.Contracts;
using Multiverse.Repository;
using Npgsql;
using System.Data;

namespace Multiverse.Postgres;

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