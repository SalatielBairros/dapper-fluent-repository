
using System.Data;
using System.Threading.Tasks;
using System;

namespace Dapper.Fluent.ORM.Contracts
{
    public interface IDapperConnection
    {
        IRepositorySettings Settings { get; }
        void Use(Action<IDbConnection> action);
        Task UseAsync(Func<IDbConnection, Task> action);
        TReturn Use<TReturn>(Func<IDbConnection, TReturn> func);
        Task<TReturn> UseAsync<TReturn>(Func<IDbConnection, Task<TReturn>> func);
        IDbConnection GetConnection();
        IDbConnection GetOpenedConnection();
        void UseTransaction(Action<IDbConnection> action);
        TReturn UseTransaction<TReturn>(Func<IDbConnection, TReturn> func);
    }
}
