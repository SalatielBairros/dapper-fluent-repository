namespace Dommel.Json;

/// <summary>
/// Extends the <see cref="ISqlBuilder"/> with support for
/// creating JSON value expressions.
/// </summary>
public interface IJsonSqlBuilder : ISqlBuilder
{
    /// <summary>
    /// Creates a JSON value expression for the specified <paramref name="column"/> and <paramref name="path"/>.
    /// </summary>
    /// <param name="column">The column which contains the JSON data.</param>
    /// <param name="path">The path of the JSON value to query.</param>
    /// <returns>A JSON value expression.</returns>
    string JsonValue(string column, string path);
}
