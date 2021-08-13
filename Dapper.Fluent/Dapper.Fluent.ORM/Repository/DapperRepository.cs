﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper.Fluent.ORM.Contracts;
using Dommel;

namespace Dapper.Fluent.ORM.Repository
{
    public abstract class DapperRepository<TEntity> : IDapperRepository<TEntity> where TEntity : class
    {
        public IDapperConnection Connection { get; }
        public DapperRepository(IDapperConnection connection)
        {
            Connection = connection;
        }

        public IEnumerable<TEntity> All() => Connection.Use(db => db.GetAll<TEntity>());

        public Task<IEnumerable<TEntity>> AllAsync() => Connection.Use(db => db.GetAllAsync<TEntity>());

        public IEnumerable<TEntity> GetData(string qry, object parameters) => Connection.Use(db => db.Query<TEntity>(qry, parameters));

        public Task<IEnumerable<TEntity>> GetDataAsync(string qry, object parameters) => Connection.Use(db => db.QueryAsync<TEntity>(qry, parameters));

        public IEnumerable<TEntity> GetData(Expression<Func<TEntity, bool>> filter) => Connection.Use(db => db.Select(filter));

        public Task<IEnumerable<TEntity>> GetDataAsync(Expression<Func<TEntity, bool>> filter) => Connection.Use(db => db.SelectAsync(filter));

        public TEntity Find(Expression<Func<TEntity, bool>> filter) => Connection.Use(db => db.FirstOrDefault(filter));

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter) => Connection.Use(db => db.FirstOrDefaultAsync(filter));

        public int Add(TEntity entity) => Connection.Use(db => (int)db.Insert(entity));

        public async Task<int> AddAsync(TEntity entity) => (int)await Connection.Use(db => db.InsertAsync(entity));

        public void Add(IEnumerable<TEntity> entities) => Connection.Use(db => db.InsertAll(entities));

        public Task AddAsync(IEnumerable<TEntity> entities) => Connection.Use(db => db.InsertAllAsync(entities));

        public void Remove(Expression<Func<TEntity, bool>> filter) => Connection.Use(db => db.DeleteMultiple(filter));

        public Task RemoveAsync(Expression<Func<TEntity, bool>> filter) => Connection.Use(db => db.DeleteMultipleAsync(filter));

        public bool Update(TEntity entity) => Connection.Use(db => db.Update(entity));

        public Task<bool> UpdateAsync(TEntity entity, object pks) => Connection.Use(db => db.UpdateAsync(entity));
    }
}
