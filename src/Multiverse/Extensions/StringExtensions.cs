namespace Multiverse.Extensions;

internal static class StringExtensions
{
    internal static string GetTableName(this string fullTableName)
    {
        if (fullTableName.Contains('.'))
        {
            return fullTableName.Split('.')[1].ToLowerInvariant();
        }
        return fullTableName;
    }
}
