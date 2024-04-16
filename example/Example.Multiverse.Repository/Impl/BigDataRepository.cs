using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper.Fluent.Application;
using Multiverse.Postgres.Contracts;

namespace Dapper.Fluent.Repository.Impl;

public class BigDataRepository : IBigDataRepository
{
    private readonly IPostgresRepository<BigData> _repository;

    public BigDataRepository(IPostgresRepository<BigData> repository)
    {
        _repository = repository;
    }

    public void InsertList(IEnumerable<BigData> data)
    {
        _repository.BulkAdd(data);
    }

    public async Task InsertListAsync(IEnumerable<BigData> data)
    {
        await _repository.BulkAddAsync(data);
    }
}