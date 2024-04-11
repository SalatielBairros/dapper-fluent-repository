using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Fluent.Domain.Contracts
{
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
}
