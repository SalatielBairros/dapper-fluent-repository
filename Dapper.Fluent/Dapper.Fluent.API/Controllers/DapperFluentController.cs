using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Fluent.Domain;
using Dapper.Fluent.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dapper.Fluent.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DapperFluentController : ControllerBase
    {
        private readonly IDapperFluentService _service;

        public DapperFluentController(IDapperFluentService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<PublicSchemaEntity> GetAll() => _service.GetAll();

        [HttpGet("{id}")]
        public PublicSchemaEntity Get(int id) => _service.Get(id);

        [HttpGet("{id}/logs")]
        public IEnumerable<LogEntity> GetLogs(int id) => _service.GetLogs(id);

        [HttpPost]
        public PublicSchemaEntity Post([FromBody] PublicSchemaEntity entity) => _service.Insert(entity);

        [HttpPatch("{id}")]
        public PublicSchemaEntity Path(int id, [FromBody] PublicSchemaEntity entity) => _service.Update(id, entity);

        [HttpPut("{id}")]
        public PublicSchemaEntity Put(int id, [FromBody] PublicSchemaEntity entity) => _service.Update(id, entity);

        [HttpDelete("{id}")]
        public void Delete(int id) => _service.Delete(id);
    }
}
