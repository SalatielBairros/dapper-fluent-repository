using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper.Fluent.Domain;
using Dapper.Fluent.ORM.Postgres.Contracts;
using Dapper.Fluent.Repository.Contracts;

namespace Dapper.Fluent.Repository.Impl;

public class BigDataRepository : IBigDataRepository
{
    private readonly IPostgresRepository<BigData> _repository;

    public BigDataRepository(IPostgresRepository<BigData> repository)
    {
        _repository = repository;
    }

    public void BulkInsert(IEnumerable<BigData> data)
    {
        _repository.BulkAdd(data);
    }

    public async Task BulkInsertAsync(IEnumerable<BigData> data)
    {
        await _repository.BulkAddAsync(data);
    }

    public void InsertList(IEnumerable<BigData> data)
    {
        _repository.AddList(data);
    }

    public async Task InsertListAsync(IEnumerable<BigData> data)
    {
        await _repository.AddListAsync(data);
    }
}