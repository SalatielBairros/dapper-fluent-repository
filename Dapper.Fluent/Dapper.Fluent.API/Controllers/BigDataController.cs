using Dapper.Fluent.Domain;
using Dapper.Fluent.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper.Fluent.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BigDataController : ControllerBase
{
    private readonly IBigDataRepository _repository;

    public BigDataController(IBigDataRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("list")]
    public IActionResult AddList(IEnumerable<BigData> data)
    {
        _repository.InsertList(data);
        return NoContent();
    }

    [HttpPost("list-async")]
    public async Task<IActionResult> AddListAsync(IEnumerable<BigData> data)
    {
        await _repository.InsertListAsync(data);
        return NoContent();
    }
}
