using Dommel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dapper.Fluent.ORM.Contracts
{
    public interface IDapperRepository<TEntity> where TEntity : class
    {
        IDapperConnection Connection { get; }
        ITableNameResolver TableNameResolver { get; }        
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        void BulkAdd(IEnumerable<TEntity> entities, int batchSize = 1000);
        Task BulkAddAsync(IEnumerable<TEntity> entities, int batchSize = 1000);
        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync();
        TEntity Find(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetData(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetData(string qry, object parameters);
        IEnumerable<TReturn> GetData<TReturn>(string qry, object parameters);
        TEntity JoinWith<TJoin>(object id, Func<TEntity, TJoin, TEntity> map);
        Task<TEntity> JoinWithAsync<TJoin>(object id, Func<TEntity, TJoin, TEntity> map);
        Task<IEnumerable<TEntity>> GetDataAsync(string qry, object parameters);
        Task<IEnumerable<TReturn>> GetDataAsync<TReturn>(string qry, object parameters);
        Task<IEnumerable<TEntity>> GetDataAsync(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetPagedData(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> filter, string orderBy = null);
        Task<TReturn> GetValueAsync<TReturn>(string sql, object parameters);
        Task<TReturn> GetValueAsync<TReturn>(Expression<Func<TEntity, object>> column, Expression<Func<TEntity, bool>> predicate = null, bool asc = false);
        void Remove(Expression<Func<TEntity, bool>> filter);
        Task RemoveAsync(Expression<Func<TEntity, bool>> filter);
        bool Update(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<int> ExecuteQueryAsync(string query, object parameters = null);
        int ExecuteQuery(string query, object parameters = null);
        Task<IEnumerable<TEntity>> GetPagedDataAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> filter, string orderBy = null);
        IEnumerable<TEntity> AllPaged(int pageNumber, int pageSize);
        Task<IEnumerable<TEntity>> AllPagedAsync(int pageNumber, int pageSize);
        void AddTransaction(IEnumerable<TEntity> entities);
        TReturn AddTransaction<TReturn>(TEntity entity);
        Task AddTransactionAsync(IEnumerable<TEntity> entities);
        Task<TReturn> AddTransactionAsync<TReturn>(TEntity entity);
        bool UpdateTransaction(TEntity entity);
        Task<bool> UpdateTransactionAsync(TEntity entity);
        void RemoveTransaction(Expression<Func<TEntity, bool>> filter);
        Task RemoveTransactionAsync(Expression<Func<TEntity, bool>> filter);
        public bool Any(Expression<Func<TEntity, bool>> filter = null);
    }
}