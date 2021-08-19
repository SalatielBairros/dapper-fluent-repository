using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dapper.Fluent.ORM.Contracts
{
    public interface IDapperRepository<TEntity> where TEntity : class
    {
        IDapperConnection Connection { get; }
        void Add(IEnumerable<TEntity> entities);
        int Add(TEntity entity);
        Task AddAsync(IEnumerable<TEntity> entities);
        Task<int> AddAsync(TEntity entity);
        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync();
        TEntity Find(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetData(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetData(string qry, object parameters);
        IEnumerable<TReturn> GetData<TReturn>(string qry, object parameters);
        TEntity JoinWith<TJoin>(object id, Func<TEntity, TJoin, TEntity> map);
        Task<TEntity> JoinWithAsync<TJoin>(object id, Func<TEntity, TJoin, TEntity> map);
        Task<IEnumerable<TEntity>> GetDataAsync(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetDataAsync(string qry, object parameters);
        void Remove(Expression<Func<TEntity, bool>> filter);
        Task RemoveAsync(Expression<Func<TEntity, bool>> filter);
        bool Update(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
    }
}