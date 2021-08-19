using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper.Fluent.ORM.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.Fluent.Mapping.Resolvers
{
    public class KeyPropertyResolver : IKeyPropertyResolver
    {
        private static readonly IKeyPropertyResolver DefaultResolver = new DefaultKeyPropertyResolver();

        public ColumnPropertyInfo[] ResolveKeyProperties(Type type)
        {
            IEntityMap entityMap;
            if (!FluentMap.FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                return DefaultResolver.ResolveKeyProperties(type);
            }

            var mapping = entityMap as IDapperFluentEntityMap;
            if (mapping != null)
            {
                var allPropertyMaps = entityMap.PropertyMaps.OfType<DapperFluentPropertyMap>();
                var keyPropertyInfos = allPropertyMaps
                     .Where(e => e.Key)
                     .Select(x => new ColumnPropertyInfo(x.PropertyInfo, x.GeneratedOption ?? (x.Key ? DatabaseGeneratedOption.Identity : DatabaseGeneratedOption.None)))
                     .ToArray();

                try
                {
                    var defaultKeyPropertyInfos = DefaultResolver.ResolveKeyProperties(type).Where(x => allPropertyMaps.Count(y => y.PropertyInfo.Equals(x.Property)) == 0);
                    keyPropertyInfos = keyPropertyInfos.Union(defaultKeyPropertyInfos).ToArray();
                } 
                catch
                {
                    if (keyPropertyInfos.Length == 0)
                    {
                        throw new InvalidOperationException($"Could not find the key properties for type '{type.FullName}'.");
                    }
                }

                return keyPropertyInfos;
            }

            return DefaultResolver.ResolveKeyProperties(type);
        }
    }
}
