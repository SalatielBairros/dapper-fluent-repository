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
        private readonly ICategoryRepository _categoryRepository;

        public DapperFluentService(IPublicSchemaEntityRepository entityRepository, ILogRepository logRepository, ICategoryRepository categoryRepository)
        {
            this._entityRepository = entityRepository;
            this._logRepository = logRepository;
            this._categoryRepository = categoryRepository;
        }

        public void Delete(int id)
        {
            if (_entityRepository.HasAny(id))
            {
                _entityRepository.Delete(id);
                _logRepository.DeleteAllByEntity(id);
            }

            throw new InvalidOperationException("No item to remove");
        }

        public PublicSchemaEntity Get(int id) => _entityRepository.Get(id);

        public PublicSchemaEntity GetWithCategory(int id) => _entityRepository.GetWithCategory(id);

        public IEnumerable<PublicSchemaEntity> GetAll() => _entityRepository.GetAll();

        public IEnumerable<LogEntity> GetLogs(int id) => _logRepository.GetAllByEntity(id);

        public void Insert(PublicSchemaEntity entity)
        {
            _categoryRepository.Insert(entity.Category);
            entity.CategoryId = 1;
            _entityRepository.Insert(entity);
            _logRepository.Insert(new LogEntity(entity));
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
