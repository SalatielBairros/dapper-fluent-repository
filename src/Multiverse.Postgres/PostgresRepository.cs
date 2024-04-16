using Multiverse.Contracts;
using Multiverse.Dommel;
using Multiverse.Postgres.Contracts;
using Multiverse.Repository;
using System;

namespace Multiverse.Postgres;

public class PostgresRepository<TEntity> : DapperRepository<TEntity>, IPostgresRepository<TEntity> where TEntity : class
{
    public PostgresRepository(IRepositorySettings options, IDapperORMRunner runner, ITableNameResolver tableNameResolver)
      : base(new PostgresDapperConnection(options), runner, tableNameResolver)
    {
    }

    public override void ConfigureDatabase()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}