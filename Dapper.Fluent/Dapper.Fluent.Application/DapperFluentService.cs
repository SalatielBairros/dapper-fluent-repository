using System;
using System.Collections.Generic;
using Dapper.Fluent.Domain;
using Dapper.Fluent.Domain.Contracts;

namespace Dapper.Fluent.Application
{
    public class DapperFluentService : IDapperFluentService
    {
        public void Delete(int id) => throw new NotImplementedException();
        public PublicSchemaEntity Get(int id) => throw new NotImplementedException();
        public IEnumerable<PublicSchemaEntity> GetAll() => throw new NotImplementedException();
        public IEnumerable<LogEntity> GetLogs(int id) => throw new NotImplementedException();
        public PublicSchemaEntity Insert(PublicSchemaEntity entity) => throw new NotImplementedException();
        public PublicSchemaEntity Update(int id, PublicSchemaEntity entity) => throw new NotImplementedException();
    }
}
