using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Dapper.Fluent.ORM.Mapping;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.Fluent.Mapping.Resolvers
{
    public class PropertyResolver : DefaultPropertyResolver
    {
        private static readonly IPropertyResolver DefaultResolver = new DefaultPropertyResolver();

        private IEnumerable<DapperFluentPropertyMap> FilterTypes(IEnumerable<DapperFluentPropertyMap> properties)
        {
            foreach (var property in properties.Where(x => !x.Ignored))
            {
                var type = property.PropertyInfo.PropertyType;
                type = Nullable.GetUnderlyingType(type) ?? type;

                if (type.GetTypeInfo().IsPrimitive || type.GetTypeInfo().IsEnum || PrimitiveTypes.Contains(type) || property.IsJson)
                {
                    yield return property;
                }
            }
        }

        public override IEnumerable<ColumnPropertyInfo> ResolveProperties(Type type)
        {
            if (FluentMapper.EntityMaps.TryGetValue(type, out IEntityMap entityMap))
            {
                var properties = entityMap.PropertyMaps.Cast<DapperFluentPropertyMap>();
                foreach (var property in FilterTypes(properties))
                {
                    yield return new ColumnPropertyInfo(property.PropertyInfo, property.GeneratedOption ?? (property.Identity ? DatabaseGeneratedOption.Identity : DatabaseGeneratedOption.None));
                }
            }
            else
            {
                foreach (var property in DefaultResolver.ResolveProperties(type))
                {
                    yield return property;
                }
            }
        }
    }
}
