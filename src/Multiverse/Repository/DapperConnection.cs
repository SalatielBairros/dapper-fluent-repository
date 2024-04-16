using System;
using System.Data;
using System.Threading.Tasks;
using Multiverse.Contracts;

namespace Multiverse.Repository;

public class DapperConnection<TAdapter> : IDapperConnection
        where TAdapter : IDbConnection, new()
{
    public IRepositorySettings Settings { get; }

    public DapperConnection(IRepositorySettings repositorySettings)
    {
        Settings = repositorySettings;
    }

    public void Use(Action<IDbConnection> action)
    {
        using var connection = GetOpenedConnection();
        action(connection);
    }

    public TReturn Use<TReturn>(Func<IDbConnection, TReturn> func)
    {
        using var connection = GetOpenedConnection();
        return func(connection);
    }

    public async Task UseAsync(Func<IDbConnection, Task> action)
    {
        using var connection = GetOpenedConnection();
        await action(connection);
    }

    public async Task<TReturn> UseAsync<TReturn>(Func<IDbConnection, Task<TReturn>> func)
    {
        using var connection = GetOpenedConnection();
        return await func(connection);
    }

    public void UseTransaction(Action<IDbConnection> action)
    {
        using var connection = GetOpenedConnection();
        using var transaction = connection.BeginTransaction();
        action(connection);
        transaction.Commit();
    }

    public TReturn UseTransaction<TReturn>(Func<IDbConnection, TReturn> func)
    {
        using var connection = GetOpenedConnection();
        using var transaction = connection.BeginTransaction();
        var ret = func(connection);
        transaction.Commit();
        return ret;
    }

    public async Task UseTransactionAsync(Func<IDbConnection, Task> action)
    {
        using var connection = GetOpenedConnection();
        using var transaction = connection.BeginTransaction();
        await action(connection);
        transaction.Commit();
    }

    public async Task<TReturn> UseTransactionAsync<TReturn>(Func<IDbConnection, Task<TReturn>> func)
    {
        using var connection = GetOpenedConnection();
        using var transaction = connection.BeginTransaction();
        var ret = await func(connection);
        transaction.Commit();
        return ret;
    }

    public virtual IDbConnection GetConnection()
    {
        return new TAdapter()
        {
            ConnectionString = Settings.ConnString
        };
    }

    public IDbConnection GetOpenedConnection()
    {
        var connection = GetConnection();
        connection.Open();
        return connection;
    }
}
