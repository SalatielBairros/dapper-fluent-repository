using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Mapping;
using Npgsql;
using System;

namespace Dapper.Fluent.ORM.Postgres;

public class PostgresJsonPropertyHandler : IJsonPropertyHandler
{
    public void SetJsonTypes(Type[] types)
    {
        NpgsqlConnection.GlobalTypeMapper.UseJsonNet(types);

        foreach (var type in types)
            SqlMapper.AddTypeHandler(type, new DefaultJsonTypeHandler());
    }
}