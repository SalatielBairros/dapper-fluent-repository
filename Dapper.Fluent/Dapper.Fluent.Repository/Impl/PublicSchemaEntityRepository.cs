using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Fluent.Domain;
using Dapper.Fluent.Repository.Contracts;
using Dommel;

namespace Dapper.Fluent.Repository.Impl
{
    public class PublicSchemaEntityRepository : IPublicSchemaEntityRepository
    {
        private readonly IDbConnection _connection;

        public PublicSchemaEntityRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Insert(PublicSchemaEntity entity)
        {
            return (int)_connection.Insert(entity);
        }

        public IEnumerable<PublicSchemaEntity> GetAll()
        {
            return _connection.GetAll<PublicSchemaEntity>();
        }
    }
}
