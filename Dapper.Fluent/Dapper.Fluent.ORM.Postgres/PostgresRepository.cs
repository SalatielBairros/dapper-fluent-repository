using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Postgres.Contracts;
using Dapper.Fluent.ORM.Repository;
using Dommel;
using System;

namespace Dapper.Fluent.ORM.Postgres;

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