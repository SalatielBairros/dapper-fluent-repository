using System.Collections.Generic;
using Dapper.Fluent.Domain;
using Dapper.Fluent.ORM.Postgres.Contracts;
using Dapper.Fluent.Repository.Contracts;

namespace Dapper.Fluent.Repository.Impl;

public class PublicSchemaEntityRepository : IPublicSchemaEntityRepository
{
    private readonly IPostgresRepository<PublicSchemaEntity> _repository;

    public PublicSchemaEntityRepository(IPostgresRepository<PublicSchemaEntity> repository)
    {
        this._repository = repository;
    }

    public bool HasAny() => _repository.Any();

    public bool HasAny(int id) => _repository.Any(x => x.Id == id);

    public void Delete(int id) => _repository.Remove(x => x.Id == id);

    public PublicSchemaEntity Get(int id) => _repository.Find(x => x.Id == id);

    public IEnumerable<PublicSchemaEntity> GetAll() => _repository.All();

    public void Insert(PublicSchemaEntity entity) => _repository.Add(entity);

    public bool Update(PublicSchemaEntity entity) => _repository.Update(entity);

    public PublicSchemaEntity GetWithCategory(int id)
        => _repository.JoinWith<Category>(id, (entity, category) =>
        {
            entity.Category = category;
            return entity;
        });

    public IEnumerable<PublicSchemaEntity> GetWithSQL(int categoryId)
        => _repository.GetData("SELECT * FROM SAMPLEENTITY WHERE CATEGORYID = :CATEGORYID", new
        {
            CategoryId = categoryId
        });
}
