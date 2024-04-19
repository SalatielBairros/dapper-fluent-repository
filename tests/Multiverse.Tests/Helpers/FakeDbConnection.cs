using Multiverse.Contracts;
using System.Data;
using System;

namespace Multiverse.Tests;

public class FakeDbConnection : IDbConnection
{
    public string ConnectionString { get; set; }

    public int ConnectionTimeout => throw new NotImplementedException();

    public string Database => throw new NotImplementedException();

    public ConnectionState State => throw new NotImplementedException();

    public IDbTransaction BeginTransaction()
    {
        throw new NotImplementedException();
    }

    public IDbTransaction BeginTransaction(IsolationLevel il)
    {
        throw new NotImplementedException();
    }

    public void ChangeDatabase(string databaseName)
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
    }

    public IDbCommand CreateCommand()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
    }

    public void Open()
    {
    }
}