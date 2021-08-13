using System.Collections.Generic;
using Dapper.Fluent.Domain;

namespace Dapper.Fluent.Repository.Contracts
{
    public interface ILogRepository
    {
        void DeleteAllByEntity(int entityId);        
        int Insert(LogEntity log);        
        IEnumerable<LogEntity> GetAllByEntity(int entityId);
    }
}
