using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Postgres.Contracts;
using Dapper.Fluent.ORM.Repository;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Dapper.Fluent.ORM.Postgres
{
    public class PostgresRepository<TEntity> : DapperRepository<TEntity>, IPostgresRepository<TEntity> where TEntity : class
    {
        public PostgresRepository(IRepositorySettings options)
          : base(new DapperConnection<NpgsqlConnection>(options))
        {
        }
    }
}
