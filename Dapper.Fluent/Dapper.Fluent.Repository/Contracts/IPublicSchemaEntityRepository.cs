using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Fluent.Domain;

namespace Dapper.Fluent.Repository.Contracts
{
    public interface IPublicSchemaEntityRepository
    {
        int Insert(PublicSchemaEntity entity);
        IEnumerable<PublicSchemaEntity> GetAll();
    }
}
