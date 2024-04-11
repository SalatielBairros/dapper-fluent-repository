using Dapper.Fluent.Domain;
using Dapper.Fluent.ORM.Postgres.Contracts;
using Dapper.Fluent.Repository.Contracts;

namespace Dapper.Fluent.Repository.Impl;

public class CategoryRepository : ICategoryRepository
{
    private readonly IPostgresRepository<Category> _repository;

    public CategoryRepository(IPostgresRepository<Category> repository)
    {
        _repository = repository;
    }

    public void Insert(Category entity) => _repository.Add(entity);
}