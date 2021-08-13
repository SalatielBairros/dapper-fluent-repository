using System;
using System.Collections.Generic;
using Dapper.Fluent.Domain;
using Dapper.Fluent.Domain.Contracts;
using Dapper.Fluent.Repository.Contracts;

namespace Dapper.Fluent.Application
{
    public class DapperFluentService : IDapperFluentService
    {
        private readonly IPublicSchemaEntityRepository _entityRepository;
        private readonly ILogRepository _logRepository;

        public DapperFluentService(IPublicSchemaEntityRepository entityRepository, ILogRepository logRepository)
        {
            this._entityRepository = entityRepository;
            this._logRepository = logRepository;
        }

        public void Delete(int id)
        {
            _entityRepository.Delete(id);
            _logRepository.DeleteAllByEntity(id);
        }

        public PublicSchemaEntity Get(int id) => _entityRepository.Get(id);
        public IEnumerable<PublicSchemaEntity> GetAll() => _entityRepository.GetAll();
        public IEnumerable<LogEntity> GetLogs(int id) => _logRepository.GetAllByEntity(id);
        public PublicSchemaEntity Insert(PublicSchemaEntity entity)
        {
            entity.Id = _entityRepository.Insert(entity);
            _logRepository.Insert(new LogEntity(entity));
            return Get(entity.Id);
        }
        public PublicSchemaEntity Update(int id, PublicSchemaEntity entity)
        {
            entity.Id = id;
            _entityRepository.Update(entity);
            _logRepository.Insert(new LogEntity(entity));
            return Get(entity.Id);
        }
    }
}
