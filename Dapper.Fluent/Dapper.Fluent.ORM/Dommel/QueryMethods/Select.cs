using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace Dapper.Fluent.ORM.Dommel;

public static partial class DommelMapper
{
    /// <summary>
    /// Selects all the entities matching the specified predicate.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="predicate">A predicate to filter the results.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="buffered">
    /// A value indicating whether the result of the query should be executed directly,
    /// or when the query is materialized (using <c>ToList()</c> for example).
    /// </param>
    /// <returns>
    /// A collection of entities of type <typeparamref name="TEntity"/> matching the specified
    /// <paramref name="predicate"/>.
    /// </returns>
    public static IEnumerable<TEntity> Select<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> predicate, ITableNameResolver tableNameResolver, IDbTransaction? transaction = null, bool buffered = true)
    {
        var sql = BuildSelectSql(connection, predicate, false, tableNameResolver, out var parameters);
        LogQuery<TEntity>(sql);
        return connection.Query<TEntity>(sql, parameters, transaction, buffered);
    }

    /// <summary>
    /// Selects all the entities matching the specified predicate.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="predicate">A predicate to filter the results.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <param name="cancellationToken">Optional cancellation token for the command.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <returns>
    /// A collection of entities of type <typeparamref name="TEntity"/> matching the specified
    /// <paramref name="predicate"/>.
    /// </returns>
    public static Task<IEnumerable<TEntity>> SelectAsync<TEntity>(
        this IDbConnection connection, Expression<Func<TEntity, bool>> predicate, ITableNameResolver tableNameResolver, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        var sql = BuildSelectSql(connection, predicate, false, tableNameResolver, out var parameters);
        LogQuery<TEntity>(sql);
        return connection.QueryAsync<TEntity>(new CommandDefinition(sql, parameters, transaction: transaction, cancellationToken: cancellationToken));
    }

    /// <summary>
    /// Selects the first entity matching the specified predicate, or a default value if no entity matched.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="predicate">A predicate to filter the results.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <returns>
    /// A instance of type <typeparamref name="TEntity"/> matching the specified
    /// <paramref name="predicate"/>.
    /// </returns>
    public static TEntity? FirstOrDefault<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> predicate,
        ITableNameResolver tableNameResolver, IDbTransaction? transaction = null)
        where TEntity : class
    {
        var sql = BuildSelectSql(connection, predicate, true, tableNameResolver, out var parameters);
        LogQuery<TEntity>(sql);
        return connection.QueryFirstOrDefault<TEntity>(sql, parameters, transaction);
    }

    /// <summary>
    /// Selects the first entity matching the specified predicate, or a default value if no entity matched.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="predicate">A predicate to filter the results.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <param name="cancellationToken">Optional cancellation token for the command.</param>
    /// <returns>
    /// A instance of type <typeparamref name="TEntity"/> matching the specified
    /// <paramref name="predicate"/>.
    /// </returns>
    public static async Task<TEntity?> FirstOrDefaultAsync<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> predicate, ITableNameResolver tableNameResolver, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var sql = BuildSelectSql(connection, predicate, true, tableNameResolver, out var parameters);
        LogQuery<TEntity>(sql);
        return await connection.QueryFirstOrDefaultAsync<TEntity>(new CommandDefinition(sql, parameters, transaction, cancellationToken: cancellationToken));
    }

    private static string BuildSelectSql<TEntity>(
        IDbConnection connection, Expression<Func<TEntity, bool>> predicate, bool firstRecordOnly, ITableNameResolver tableNameResolver, out DynamicParameters parameters)
    {
        var type = typeof(TEntity);

        // Build the select all part
        var sql = BuildGetAllQuery(connection, type, tableNameResolver);

        // Append the where statement
        var sqlBuilder = GetSqlBuilder(connection);
        var sqlExpression = CreateSqlExpression<TEntity>(sqlBuilder)
            .Where(predicate);

        sql += sqlExpression.ToSql(out parameters);

        if (firstRecordOnly)
        {
            sql += $" {sqlBuilder.LimitClause(1)}";
        }

        return sql;
    }

    /// <summary>
    /// Selects all the entities matching the specified predicate.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="predicate">A predicate to filter the results.</param>
    /// <param name="pageNumber">The number of the page to fetch, starting at 1.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <param name="tableNameResolver">Injection of Table name resolver.</param>
    /// <param name="orderBy">Order by query with or without the order by statement.</param>
    /// <param name="buffered">
    /// A value indicating whether the result of the query should be executed directly,
    /// or when the query is materialized (using <c>ToList()</c> for example).
    /// </param>
    /// <returns>
    /// A collection of entities of type <typeparamref name="TEntity"/> matching the specified
    /// <paramref name="predicate"/>.
    /// </returns>
    public static IEnumerable<TEntity> SelectPaged<TEntity>(
        this IDbConnection connection, 
        Expression<Func<TEntity, bool>> predicate, 
        int pageNumber, 
        int pageSize, 
        ITableNameResolver tableNameResolver, 
        IDbTransaction? transaction = null, 
        bool buffered = true, 
        string orderBy = null)
    {
        var sql = BuildSelectPagedQuery(connection, predicate, pageNumber, pageSize, tableNameResolver, out var parameters, orderBy);
        LogQuery<TEntity>(sql);
        return connection.Query<TEntity>(sql, parameters, transaction, buffered);
    }

    /// <summary>
    /// Selects all the entities matching the specified predicate.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="connection">The connection to the database. This can either be open or closed.</param>
    /// <param name="predicate">A predicate to filter the results.</param>
    /// <param name="pageNumber">The number of the page to fetch, starting at 1.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="transaction">Optional transaction for the command.</param>
    /// <param name="cancellationToken">Optional cancellation token for the command.</param>
    /// <param name="tableNameResolver">Table name resolver injection.</param>
    /// <param name="orderBy">Order by query with or without the order by statement.</param>
    /// <returns>
    /// A collection of entities of type <typeparamref name="TEntity"/> matching the specified
    /// <paramref name="predicate"/>.
    /// </returns>
    public static Task<IEnumerable<TEntity>> SelectPagedAsync<TEntity>(
        this IDbConnection connection, 
        Expression<Func<TEntity, bool>> predicate, 
        int pageNumber,
        int pageSize, 
        ITableNameResolver tableNameResolver,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default,
        string orderBy = null)
    {
        var sql = BuildSelectPagedQuery(connection, predicate, pageNumber, pageSize, tableNameResolver, out var parameters, orderBy);
        LogQuery<TEntity>(sql);
        return connection.QueryAsync<TEntity>(new CommandDefinition(sql, parameters, transaction, cancellationToken: cancellationToken));
    }

    private static string BuildSelectPagedQuery<TEntity>(
        IDbConnection connection, 
        Expression<Func<TEntity, bool>> predicate, 
        int pageNumber, 
        int pageSize, 
        ITableNameResolver tableNameResolver, 
        out DynamicParameters parameters,
        string orderBy = null)
    {
        // Start with the select query part
        var sql = BuildSelectSql(connection, predicate, false, tableNameResolver, out parameters);

        if (string.IsNullOrWhiteSpace(orderBy))
        {
            var keyColumns = Resolvers.KeyProperties(typeof(TEntity)).Select(p => Resolvers.Column(p.Property, connection));
            orderBy = "order by " + string.Join(", ", keyColumns);
        }
        else if (!orderBy.Contains("order by"))
            orderBy = $"order by {orderBy}";
        
        sql += GetSqlBuilder(connection).BuildPaging(orderBy, pageNumber, pageSize);
        return sql;
    }

    public static string GetWhereSql<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> predicate, out DynamicParameters parameters)
    {
        return CreateSqlExpression<TEntity>(GetSqlBuilder(connection))
            .Where(predicate)
            .ToSql(out parameters);
    }

    public static string GetFirstSql(this IDbConnection connection, string orderByColumn, string direction = "asc")
    {
        var orderBy = $"order by {orderByColumn} {direction}";
        return GetSqlBuilder(connection).BuildPaging(orderBy, 1, 1);
    }
}
