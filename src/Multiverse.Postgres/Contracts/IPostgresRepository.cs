using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiverse.Contracts;

namespace Multiverse.Postgres.Contracts;

public interface IPostgresRepository<TEntity> : IDapperRepository<TEntity> where TEntity : class
{

}
