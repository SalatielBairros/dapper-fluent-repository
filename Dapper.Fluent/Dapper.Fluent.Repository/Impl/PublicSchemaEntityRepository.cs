using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Fluent.Domain;
using Dapper.Fluent.ORM.Postgres;
using Dapper.Fluent.ORM.Postgres.Contracts;
using Dapper.Fluent.ORM.Repository;
using Dapper.Fluent.Repository.Contracts;
using Dommel;

namespace Dapper.Fluent.Repository.Impl
{
    public class PublicSchemaEntityRepository : IPublicSchemaEntityRepository
    {
        private readonly IPostgresRepository<PublicSchemaEntity> _repository;

        public PublicSchemaEntityRepository(IPostgresRepository<PublicSchemaEntity> repository)
        {
            this._repository = repository;
        }

        public IEnumerable<PublicSchemaEntity> GetAll() => _repository.All();
        public int Insert(PublicSchemaEntity entity) => _repository.Add(entity);
    }
}
