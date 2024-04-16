using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Fluent.ORM.Contracts;

namespace Dapper.Fluent.ORM.Postgres.Contracts;

public interface IPostgresRepository<TEntity> : IDapperRepository<TEntity> where TEntity : class
{

}
