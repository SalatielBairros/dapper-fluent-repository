using Dapper.FluentMap.Mapping;
using System.Collections.Concurrent;
using System;
using System.Linq;

namespace Multiverse.Mapping;

public static class FluentMapExtensions
{
    public static bool TryGetMap(this ConcurrentDictionary<Type, IEntityMap> map, Type type, out IEntityMap entityMap)
    {
        if (map.TryGetValue(type, out entityMap)) return true;

        entityMap = map.FirstOrDefault(m => m.Key.BaseType == type).Value;
        return entityMap != null;
    }
}