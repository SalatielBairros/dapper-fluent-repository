using System.Collections.Generic;
using Dapper.Fluent.Domain;

namespace Dapper.Fluent.Repository.Contracts;

public interface ILogRepository
{
    void DeleteAllByEntity(int entityId);        
    void Insert(LogEntity log);        
    IEnumerable<LogEntity> GetAllByEntity(int entityId);
}
