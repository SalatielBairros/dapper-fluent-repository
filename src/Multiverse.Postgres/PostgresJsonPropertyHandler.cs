using Dapper;
using Multiverse.Contracts;
using Multiverse.Mapping;
using Npgsql;
using System;

namespace Multiverse.Postgres;

public class PostgresJsonPropertyHandler : IJsonPropertyHandler
{
    public void SetJsonTypes(Type[] types)
    {
        NpgsqlConnection.GlobalTypeMapper.UseJsonNet(null, types);

        foreach (var type in types)
            SqlMapper.AddTypeHandler(type, new DefaultJsonTypeHandler());
    }
}