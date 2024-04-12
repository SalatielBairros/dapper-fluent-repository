using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace Dommel;

public static partial class DommelMapper
{
    /// <summary>
    /// Retrieves the entity of type <typeparamref name="TEntity"/> with the specified id.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="id">The id of the entity in the database.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <returns>The entity with the corresponding id.</returns>
    public static TEntity? Get<TEntity>(this IDbConnection connection, object id, ITableNameResolver tableNameResolver, IDbTransaction? transaction = null)
        where TEntity : class
    {
        var sql = BuildGetById(GetSqlBuilder(connection), typeof(TEntity), id, tableNameResolver, out var parameters);
        LogQuery<TEntity>(sql);
        return connection.QueryFirstOrDefault<TEntity>(sql, parameters, transaction);
    }

    /// <summary>
    /// Retrieves the entity of type <typeparamref name="TEntity"/> with the specified id.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="id">The id of the entity in the database.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    ///  <param name="cancellationToken">Optional cancellation token for the command.</param>
    /// <returns>The entity with the corresponding id.</returns>
    public static async Task<TEntity?> GetAsync<TEntity>(this IDbConnection connection, object id, ITableNameResolver tableNameResolver, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var sql = BuildGetById(GetSqlBuilder(connection), typeof(TEntity), id, tableNameResolver, out var parameters);
        LogQuery<TEntity>(sql);
        return await connection.QueryFirstOrDefaultAsync<TEntity>(new CommandDefinition(sql, parameters, transaction: transaction, cancellationToken: cancellationToken));
    }

    internal static string BuildGetById(ISqlBuilder sqlBuilder, Type type, object id, ITableNameResolver tableNameResolver, out DynamicParameters parameters)
    {
        var tableName = Resolvers.Table(type, sqlBuilder, tableNameResolver);
        var cacheKey = new QueryCacheKey(QueryCacheType.Get, sqlBuilder, type, tableName);
        if (!QueryCache.TryGetValue(cacheKey, out var sql))
        {
            var keyProperties = Resolvers.KeyProperties(type);
            if (keyProperties.Length > 1)
            {
                throw new InvalidOperationException($"Entity {type.Name} contains more than one key property." +
                    "Use the Get<T> overload which supports passing multiple IDs.");
            }
            var keyColumnName = Resolvers.Column(keyProperties[0].Property, sqlBuilder);

            sql = $"select * from {tableName} where {keyColumnName} = {sqlBuilder.PrefixParameter("Id")}";
            QueryCache.TryAdd(cacheKey, sql);
        }

        parameters = new DynamicParameters();
        parameters.Add("Id", id);

        return sql;
    }

    /// <summary>
    /// Retrieves the entity of type <typeparamref name="TEntity"/> with the specified id.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="ids">The id of the entity in the database.</param>
    /// <returns>The entity with the corresponding id.</returns>
    public static TEntity? Get<TEntity>(this IDbConnection connection, ITableNameResolver tableNameResolver, params object[] ids) where TEntity : class
        => Get<TEntity>(connection, tableNameResolver, ids, transaction: null);

    /// <summary>
    /// Retrieves the entity of type <typeparamref name="TEntity"/> with the specified id.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="ids">The id of the entity in the database.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <returns>The entity with the corresponding id.</returns>
    public static TEntity? Get<TEntity>(this IDbConnection connection, ITableNameResolver tableNameResolver, object[] ids, IDbTransaction? transaction = null) where TEntity : class
    {
        if (ids.Length == 1)
        {
            return Get<TEntity>(connection, ids[0], tableNameResolver, transaction);
        }

        var sql = BuildGetByIds(connection, typeof(TEntity), tableNameResolver, ids, out var parameters);
        LogQuery<TEntity>(sql);
        return connection.QueryFirstOrDefault<TEntity>(sql, parameters, transaction);
    }

    /// <summary>
    /// Retrieves the entity of type <typeparamref name="TEntity"/> with the specified id.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="ids">The id of the entity in the database.</param>
    /// <returns>The entity with the corresponding id.</returns>
    public static Task<TEntity?> GetAsync<TEntity>(this IDbConnection connection, ITableNameResolver tableNameResolver, params object[] ids) where TEntity : class
        => GetAsync<TEntity>(connection, ids, tableNameResolver, transaction: null, cancellationToken: default);

    /// <summary>
    /// Retrieves the entity of type <typeparamref name="TEntity"/> with the specified id.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="ids">The id of the entity in the database.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <param name="cancellationToken">Optional cancellation token for the command.</param>
    /// <returns>The entity with the corresponding id.</returns>
    public static async Task<TEntity?> GetAsync<TEntity>(this IDbConnection connection, object[] ids, ITableNameResolver tableNameResolver, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        if (ids.Length == 1)
        {
            return await GetAsync<TEntity>(connection, ids[0], tableNameResolver, transaction, cancellationToken);
        }

        var sql = BuildGetByIds(connection, typeof(TEntity), tableNameResolver, ids, out var parameters);
        LogQuery<TEntity>(sql);
        return await connection.QueryFirstOrDefaultAsync<TEntity>(new CommandDefinition(sql, parameters, transaction, cancellationToken: cancellationToken));
    }

    internal static string BuildGetByIds(IDbConnection connection, Type type, ITableNameResolver tableNameResolver, object[] ids, out DynamicParameters parameters)
    {
        var sqlBuilder = GetSqlBuilder(connection);
        var tableName = Resolvers.Table(type, sqlBuilder, tableNameResolver);
        var cacheKey = new QueryCacheKey(QueryCacheType.GetByMultipleIds, sqlBuilder, type, tableName);
        if (!QueryCache.TryGetValue(cacheKey, out var sql))
        {
            var keyProperties = Resolvers.KeyProperties(type);
            var keyColumnNames = keyProperties.Select(p => Resolvers.Column(p.Property, sqlBuilder)).ToArray();
            if (keyColumnNames.Length != ids.Length)
            {
                throw new InvalidOperationException($"Number of key columns ({keyColumnNames.Length}) of type {type.Name} does not match with the number of specified IDs ({ids.Length}).");
            }

            var sb = new StringBuilder("select * from ").Append(tableName).Append(" where");
            var i = 0;
            foreach (var keyColumnName in keyColumnNames)
            {
                if (i != 0)
                {
                    sb.Append(" and");
                }

                sb.Append(' ').Append(keyColumnName).Append($" = {sqlBuilder.PrefixParameter("Id")}").Append(i);
                i++;
            }

            sql = sb.ToString();
            QueryCache.TryAdd(cacheKey, sql);
        }

        parameters = new DynamicParameters();
        for (var i = 0; i < ids.Length; i++)
        {
            parameters.Add("Id" + i, ids[i]);
        }

        return sql;
    }

    /// <summary>
    /// Retrieves all the entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="buffered">
    /// A value indicating whether the result of the query should be executed directly,
    /// or when the query is materialized (using <c>ToList()</c> for example).
    /// </param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <returns>A collection of entities of type <typeparamref name="TEntity"/>.</returns>
    public static IEnumerable<TEntity> GetAll<TEntity>(this IDbConnection connection, ITableNameResolver tableNameResolver, IDbTransaction? transaction = null, bool buffered = true) where TEntity : class
    {
        var sql = BuildGetAllQuery(connection, typeof(TEntity), tableNameResolver);
        LogQuery<TEntity>(sql);
        return connection.Query<TEntity>(sql, transaction: transaction, buffered: buffered);
    }

    /// <summary>
    /// Retrieves all the entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <param name="cancellationToken">Optional cancellation token for the command.</param>
    /// <returns>A collection of entities of type <typeparamref name="TEntity"/>.</returns>
    public static Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(this IDbConnection connection, ITableNameResolver tableNameResolver, IDbTransaction? transaction = null, CancellationToken cancellationToken = default) where TEntity : class
    {
        var sql = BuildGetAllQuery(connection, typeof(TEntity), tableNameResolver);
        LogQuery<TEntity>(sql);
        return connection.QueryAsync<TEntity>(new CommandDefinition(sql, transaction: transaction, cancellationToken: cancellationToken));
    }

    internal static string BuildGetAllQuery(IDbConnection connection, Type type, ITableNameResolver tableNameResolver)
    {
        var sqlBuilder = GetSqlBuilder(connection);
        var tableName = Resolvers.Table(type, sqlBuilder, tableNameResolver);
        var cacheKey = new QueryCacheKey(QueryCacheType.GetAll, sqlBuilder, type, tableName);
        if (!QueryCache.TryGetValue(cacheKey, out var sql))
        {
            sql = "select * from " + tableName;
            QueryCache.TryAdd(cacheKey, sql);
        }

        return sql;
    }

    /// <summary>
    /// Retrieves a paged set of entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="pageNumber">The number of the page to fetch, starting at 1.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="buffered">
    /// A value indicating whether the result of the query should be executed directly,
    /// or when the query is materialized (using <c>ToList()</c> for example).
    /// </param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <returns>A paged collection of entities of type <typeparamref name="TEntity"/>.</returns>
    public static IEnumerable<TEntity> GetPaged<TEntity>(this IDbConnection connection, int pageNumber, int pageSize, ITableNameResolver tableNameResolver, IDbTransaction? transaction = null, bool buffered = true) where TEntity : class
    {
        var sql = BuildPagedQuery(connection, typeof(TEntity), pageNumber, pageSize, tableNameResolver);
        LogQuery<TEntity>(sql);
        return connection.Query<TEntity>(sql, transaction: transaction, buffered: buffered);
    }

    /// <summary>
    /// Retrieves a paged set of entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="pageNumber">The number of the page to fetch, starting at 1.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <param name="cancellationToken">Optional cancellation token for the command.</param>
    /// <returns>A paged collection of entities of type <typeparamref name="TEntity"/>.</returns>
    public static Task<IEnumerable<TEntity>> GetPagedAsync<TEntity>(this IDbConnection connection, int pageNumber, int pageSize, ITableNameResolver tableNameResolver, IDbTransaction? transaction = null, CancellationToken cancellationToken = default) where TEntity : class
    {
        var sql = BuildPagedQuery(connection, typeof(TEntity), pageNumber, pageSize, tableNameResolver);
        LogQuery<TEntity>(sql);
        return connection.QueryAsync<TEntity>(new CommandDefinition(sql, transaction: transaction, cancellationToken: cancellationToken));
    }

    internal static string BuildPagedQuery(IDbConnection connection, Type type, int pageNumber, int pageSize, ITableNameResolver tableNameResolver)
    {
        // Start with the select query part
        var sql = BuildGetAllQuery(connection, type, tableNameResolver);

        // Append the paging part including the order by
        var keyColumns = Resolvers.KeyProperties(type).Select(p => Resolvers.Column(p.Property, connection));
        var orderBy = "order by " + string.Join(", ", keyColumns);
        sql += GetSqlBuilder(connection).BuildPaging(orderBy, pageNumber, pageSize);
        return sql;
    }
}