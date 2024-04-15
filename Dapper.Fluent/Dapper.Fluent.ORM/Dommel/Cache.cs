using System;
using System.Reflection;

namespace Dapper.Fluent.ORM.Dommel;

internal readonly struct QueryCacheKey : IEquatable<QueryCacheKey>
{
    public QueryCacheKey(QueryCacheType cacheType, ISqlBuilder sqlBuilder, MemberInfo memberInfo, string tableName)
    {
        SqlBuilderType = sqlBuilder.GetType();
        CacheType = cacheType;
        MemberInfo = memberInfo;
        TableName = tableName;
    }

    public QueryCacheType CacheType { get; }

    public Type SqlBuilderType { get; }

    public MemberInfo MemberInfo { get; }

    public string TableName { get; }

    public bool Equals(QueryCacheKey other) =>
            CacheType == other.CacheType &&
            SqlBuilderType == other.SqlBuilderType &&
            MemberInfo == other.MemberInfo &&
            TableName == other.TableName;

    public override bool Equals(object? obj) => obj is QueryCacheKey key && Equals(key);

    public override int GetHashCode() => CacheType.GetHashCode() + SqlBuilderType.GetHashCode() + MemberInfo.GetHashCode();
}
