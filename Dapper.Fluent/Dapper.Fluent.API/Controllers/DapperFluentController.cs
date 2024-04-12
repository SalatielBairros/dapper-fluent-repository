using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Fluent.Domain;
using Dapper.Fluent.Domain.Contracts;
using Dapper.Fluent.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dapper.Fluent.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DapperFluentController : ControllerBase
{
    private readonly IDapperFluentService _service;
    private readonly IPublicSchemaEntityRepository _entityRepository;

    public DapperFluentController(IDapperFluentService service, IPublicSchemaEntityRepository entityRepository)
    {
        _service = service;
        _entityRepository = entityRepository;
    }

    [HttpGet]
    public IEnumerable<PublicSchemaEntity> GetAll() => _service.GetAll();

    [HttpGet("any")]
    public object HasAnyEntity()
    {
        return new
        {
            HasEntity = _entityRepository.HasAny()
        };
    }

    [HttpGet("{id}")]
    public PublicSchemaEntity Get(int id) => _service.Get(id);

    [HttpGet("{id}/with-category")]
    public PublicSchemaEntity GetWithCategory(int id) => _service.GetWithCategory(id);

    [HttpGet("{id}/logs")]
    public IEnumerable<LogEntity> GetLogs(int id) => _service.GetLogs(id);

    [HttpPost]
    public void Post([FromBody] PublicSchemaEntity entity) => _service.Insert(entity);

    [HttpPatch("{id}")]
    public PublicSchemaEntity Path(int id, [FromBody] PublicSchemaEntity entity) => _service.Update(id, entity);

    [HttpPut("{id}")]
    public PublicSchemaEntity Put(int id, [FromBody] PublicSchemaEntity entity) => _service.Update(id, entity);

    [HttpDelete("{id}")]
    public void Delete(int id) => _service.Delete(id);
}
