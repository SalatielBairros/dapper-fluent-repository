using Dapper.Fluent.Domain;
using System.Collections.Generic;

namespace Dapper.Fluent.Application;

public interface IDapperFluentService
{
    PublicSchemaEntity Get(int id);
    PublicSchemaEntity GetWithCategory(int id);
    IEnumerable<PublicSchemaEntity> GetAll();
    IEnumerable<LogEntity> GetLogs(int id);
    void Insert(PublicSchemaEntity entity);
    PublicSchemaEntity Update(int id, PublicSchemaEntity entity);
    void Delete(int id);
}
