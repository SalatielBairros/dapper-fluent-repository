using System.Linq;
using System.Reflection;
using Dapper.Fluent.ORM.Mapping;
using Dommel;

namespace Dapper.Fluent.Mapping.Resolvers
{
    /// <summary>
    /// Implements the <see cref="IColumnNameResolver"/> interface by using the configured mapping.
    /// </summary>
    public class ColumnNameResolver : IColumnNameResolver
    {
        private static readonly IColumnNameResolver DefaultResolver = new DefaultColumnNameResolver();

        /// <inheritdoc/>
        public string ResolveColumnName(PropertyInfo propertyInfo)
        {
            if (propertyInfo.DeclaringType != null)
            {
                if (FluentMap.FluentMapper.EntityMaps.TryGetValue(propertyInfo.ReflectedType, out var entityMap))
                {
                    var mapping = entityMap as IDapperFluentEntityMap;
                    if (mapping != null)
                    {
                        var propertyMaps = entityMap.PropertyMaps.Where(m => m.PropertyInfo.Name == propertyInfo.Name).ToList();
                        if (propertyMaps.Count == 1)
                        {
                            return propertyMaps[0].ColumnName;
                        }
                    }
                }
                else if (FluentMap.FluentMapper.TypeConventions.TryGetValue(propertyInfo.ReflectedType, out var conventions))
                {
                    foreach (var convention in conventions)
                    {
                        var propertyMaps = convention.PropertyMaps.Where(m => m.PropertyInfo.Name == propertyInfo.Name).ToList();
                        if (propertyMaps.Count == 1)
                        {
                            return propertyMaps[0].ColumnName;
                        }
                    }
                }
            }

            return DefaultResolver.ResolveColumnName(propertyInfo);
        }
    }
}
