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
        void Delete(int id);
        bool Update(PublicSchemaEntity entity);
        int Insert(PublicSchemaEntity entity);
        IEnumerable<PublicSchemaEntity> GetAll();
        PublicSchemaEntity Get(int id);
    }
}
