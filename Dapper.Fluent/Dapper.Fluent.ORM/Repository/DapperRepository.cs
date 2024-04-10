using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper.Fluent.Mapping.Resolvers;
using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Extensions;
using Dommel;

namespace Dapper.Fluent.ORM.Repository;

public abstract class DapperRepository<TEntity> : IDapperRepository<TEntity> where TEntity : class
{
    public ITableNameResolver TableNameResolver { get; }
    public IDapperConnection Connection { get; }

    protected DapperRepository(IDapperConnection connection, IDapperORMRunner runner, ITableNameResolver tableNameResolver)
    {
        TableNameResolver = tableNameResolver;
        ConfigureDatabase();
        runner.CreateTablesFromMigrations();
        Connection = connection;
    }

    public virtual void ConfigureDatabase() { }

    #region All

    public IEnumerable<TEntity> All() => Connection.Use(db => db.GetAll<TEntity>(TableNameResolver));

    public Task<IEnumerable<TEntity>> AllAsync() => Connection.UseAsync(db => db.GetAllAsync<TEntity>(TableNameResolver));

    public IEnumerable<TEntity> AllPaged(int pageNumber, int pageSize) => Connection.Use(db => db.GetPaged<TEntity>(pageNumber, pageSize, TableNameResolver));

    public Task<IEnumerable<TEntity>> AllPagedAsync(int pageNumber, int pageSize) => Connection.UseAsync(db => db.GetPagedAsync<TEntity>(pageNumber, pageSize, TableNameResolver));

    #endregion

    #region JoinWith

    public TEntity JoinWith<TJoin>(object id, Func<TEntity, TJoin, TEntity> map) => Connection.Use(db => db.Get(id, map, TableNameResolver));

    public Task<TEntity> JoinWithAsync<TJoin>(object id, Func<TEntity, TJoin, TEntity> map) => Connection.UseAsync(db => db.GetAsync(id, map, TableNameResolver));

    #endregion

    #region GetData

    public IEnumerable<TEntity> GetData(string qry, object parameters) => Connection.Use(db => db.Query<TEntity>(qry, parameters));

    public IEnumerable<TReturn> GetData<TReturn>(string qry, object parameters) => Connection.Use(db => db.Query<TReturn>(qry, parameters));

    public IEnumerable<TEntity> GetData(Expression<Func<TEntity, bool>> filter) => Connection.Use(db => db.Select(filter, TableNameResolver));

    public Task<IEnumerable<TEntity>> GetDataAsync(string qry, object parameters) => Connection.UseAsync(db => db.QueryAsync<TEntity>(qry, parameters));

    public Task<IEnumerable<TReturn>> GetDataAsync<TReturn>(string qry, object parameters) => Connection.UseAsync(db => db.QueryAsync<TReturn>(qry, parameters));

    public async Task<IEnumerable<TEntity>> GetDataAsync(Expression<Func<TEntity, bool>> filter) => await Task.Run(() => Connection.Use(db => db.Select(filter, TableNameResolver)));

    public IEnumerable<TEntity> GetPagedData(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> filter, string orderBy = null)
        => Connection.Use(db => db.SelectPaged(filter, pageNumber, pageSize, TableNameResolver, null, true, orderBy));

    public Task<IEnumerable<TEntity>> GetPagedDataAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> filter, string orderBy = null)
        => Connection.UseAsync(db => db.SelectPagedAsync(filter, pageNumber, pageSize, TableNameResolver, null, default, orderBy));

    public Task<TReturn> GetValueAsync<TReturn>(string sql, object parameters)
        => Connection.UseAsync(db => db.ExecuteScalarAsync<TReturn>(sql, parameters));

    public Task<TReturn> GetValueAsync<TReturn>(Expression<Func<TEntity, object>> column, Expression<Func<TEntity, bool>> predicate = null, bool asc = false)
    {
        var propertyName = ((UnaryExpression)column.Body).Operand.ToString().Split(".").Last();
        var property = typeof(TEntity).GetProperty(propertyName);
        var columnName = new ColumnNameResolver().ResolveColumnName(property);
        var tableName = TableNameResolver.ResolveTableName(typeof(TEntity));

        return Connection.UseAsync(db =>
        {
            var whereClause = string.Empty;
            var parameters = new DynamicParameters();
            if (predicate != null)
                whereClause = db.GetWhereSql(predicate, out parameters);
            var direction = asc ? "asc" : "desc";
            var paging = db.GetFirstSql(columnName, direction);
            var selectQuery = $"select {columnName} from {tableName} {whereClause} {paging}";
            return db.ExecuteScalarAsync<TReturn>(selectQuery, parameters);
        });
    }

    #endregion

    #region Find

    public TEntity Find(Expression<Func<TEntity, bool>> filter) => Connection.Use(db => db.FirstOrDefault(filter, TableNameResolver));

    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter) => await Task.Run(() => Connection.Use(db => db.FirstOrDefault(filter, TableNameResolver)));

    #endregion

    #region Add

    public TReturn Add<TReturn>(TEntity entity)
    {
        EntityValidation.ThrowIfErrorOn(entity);
        return Connection.Use(db => (TReturn)db.Insert(entity, TableNameResolver));
    }

    public void Add(IEnumerable<TEntity> entities)
    {
        var data = entities.ToList();
        data.ForEach(EntityValidation.ThrowIfErrorOn);
        Connection.Use(db => db.InsertAll(data, TableNameResolver));
    }

    public Task AddAsync(IEnumerable<TEntity> entities)
    {
        entities.ToList().ForEach(EntityValidation.ThrowIfErrorOn);
        return Connection.UseAsync(db => db.InsertAllAsync(entities, TableNameResolver));
    }

    public async Task<TReturn> AddAsync<TReturn>(TEntity entity) => await Task.Run(() => Add<TReturn>(entity));

    public void BulkAdd(IEnumerable<TEntity> entities, int batchSize = 1000)
    {
        var data = entities.ToList();
        data.ForEach(EntityValidation.ThrowIfErrorOn);

        Connection.Use(c =>
        {
            var transaction = c.BeginTransaction();
            var builder = DommelMapper.GetSqlBuilder(c);
            var sql = DommelMapper.BuildInsertQuery(builder, typeof(TEntity), TableNameResolver, false);
            c.BulkInsert(
                $"{sql}...",
                data,
                transaction,
                batchSize);
            transaction.Commit();
        });
    }

    public async Task BulkAddAsync(IEnumerable<TEntity> entities, int batchSize = 1000)
    {
        var data = entities.ToList();
        data.ForEach(EntityValidation.ThrowIfErrorOn);

        await Connection.UseAsync(async c =>
        {
            var transaction = c.BeginTransaction();
            var builder = DommelMapper.GetSqlBuilder(c);
            var sql = DommelMapper.BuildInsertQuery(builder, typeof(TEntity), TableNameResolver, false);
            await c.BulkInsertAsync(
                $"{sql}...",
                data,
                transaction,
                batchSize);
            transaction.Commit();
        });
    }

    #endregion

    #region Remove

    public void Remove(Expression<Func<TEntity, bool>> filter) => Connection.Use(db => db.DeleteMultiple(filter, TableNameResolver));

    public Task RemoveAsync(Expression<Func<TEntity, bool>> filter) => Connection.UseAsync(db => db.DeleteMultipleAsync(filter, TableNameResolver));

    #endregion

    #region Update

    public bool Update(TEntity entity)
    {
        EntityValidation.ThrowIfErrorOn(entity);
        return Connection.Use(db => db.Update(entity, TableNameResolver));
    }

    public Task<bool> UpdateAsync(TEntity entity)
    {
        EntityValidation.ThrowIfErrorOn(entity);
        return Connection.UseAsync(db => db.UpdateAsync(entity, TableNameResolver));
    }

    #endregion

    #region ExecuteQuery

    public Task<int> ExecuteQueryAsync(string query, object parameters = null)
        => Connection.UseAsync(db => db.ExecuteAsync(query, parameters));

    public int ExecuteQuery(string query, object parameters = null) => Connection.Use(db => db.Execute(query, parameters));

    #endregion

    #region Add Using Transaction

    public void AddTransaction(IEnumerable<TEntity> entities)
    {
        entities.ToList().ForEach(EntityValidation.ThrowIfErrorOn);
        Connection.UseTransaction(db => db.InsertAll(entities, TableNameResolver));
    }

    public TReturn AddTransaction<TReturn>(TEntity entity)
    {
        EntityValidation.ThrowIfErrorOn(entity);
        return Connection.UseTransaction(db => (TReturn)db.Insert(entity, TableNameResolver));
    }

    public async Task AddTransactionAsync(IEnumerable<TEntity> entities)
    {
        entities.ToList().ForEach(EntityValidation.ThrowIfErrorOn);
        await Connection.UseTransactionAsync(async db => await db.InsertAllAsync(entities, TableNameResolver));
    }

    public async Task<TReturn> AddTransactionAsync<TReturn>(TEntity entity)
    {
        EntityValidation.ThrowIfErrorOn(entity);
        return await Connection.UseTransactionAsync(async db => (TReturn)await db.InsertAsync(entity, TableNameResolver));
    }

    #endregion

    #region Update Using Transaction

    public bool UpdateTransaction(TEntity entity)
    {
        EntityValidation.ThrowIfErrorOn(entity);
        return Connection.UseTransaction(db => db.Update(entity, TableNameResolver));
    }

    public async Task<bool> UpdateTransactionAsync(TEntity entity)
    {
        EntityValidation.ThrowIfErrorOn(entity);
        return await Connection.UseTransactionAsync(async db => await db.UpdateAsync(entity, TableNameResolver));
    }

    #endregion

    #region Remove Using Transaction

    public void RemoveTransaction(Expression<Func<TEntity, bool>> filter)
    {
        Connection.UseTransaction(db => db.DeleteMultiple(filter, TableNameResolver));
    }

    public async Task RemoveTransactionAsync(Expression<Func<TEntity, bool>> filter)
    {
        await Connection.UseTransactionAsync(async db => await db.DeleteMultipleAsync(filter, TableNameResolver));
    }

    #endregion
}
