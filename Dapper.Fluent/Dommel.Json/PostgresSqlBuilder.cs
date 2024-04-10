namespace Dommel.Json;

/// <summary>
/// JSON SQL builder for PostgreSQL.
/// </summary>
public class PostgresSqlBuilder : Dommel.PostgresSqlBuilder, IJsonSqlBuilder
{
    /// <inheritdoc />
    public string JsonValue(string column, string path) => $"{column}->>'{path}'";
}