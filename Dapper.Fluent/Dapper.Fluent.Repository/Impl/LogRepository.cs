using System.Collections.Generic;
using Dapper.Fluent.Domain;
using Dapper.Fluent.ORM.Postgres.Contracts;
using Dapper.Fluent.Repository.Contracts;

namespace Dapper.Fluent.Repository.Impl
{
    public class LogRepository : ILogRepository
    {
        private readonly IPostgresRepository<LogEntity> _repository;

        public LogRepository(IPostgresRepository<LogEntity> repository)
        {
            this._repository = repository;
        }

        public void DeleteAllByEntity(int entityId) => _repository.Remove(x => x.PublicId == entityId);
        public IEnumerable<LogEntity> GetAllByEntity(int entityId) => _repository.GetData(x => x.PublicId == entityId);
        public void Insert(LogEntity log) => _repository.Add(log);
    }
}
