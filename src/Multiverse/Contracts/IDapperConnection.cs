
using System.Data;
using System.Threading.Tasks;
using System;

namespace Multiverse.Contracts;

public interface IDapperConnection
{
    IRepositorySettings Settings { get; }
    void Use(Action<IDbConnection> action);
    TReturn Use<TReturn>(Func<IDbConnection, TReturn> func);
    Task UseAsync(Func<IDbConnection, Task> action);
    Task<TReturn> UseAsync<TReturn>(Func<IDbConnection, Task<TReturn>> func);
    IDbConnection GetConnection();
    IDbConnection GetOpenedConnection();
    void UseTransaction(Action<IDbConnection> action);
    TReturn UseTransaction<TReturn>(Func<IDbConnection, TReturn> func);
    Task UseTransactionAsync(Func<IDbConnection, Task> action);
    Task<TReturn> UseTransactionAsync<TReturn>(Func<IDbConnection, Task<TReturn>> func);
}
