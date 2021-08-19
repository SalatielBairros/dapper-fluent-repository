using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Dapper.Fluent.ORM.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.Fluent.Mapping.Resolvers
{
    public class PropertyResolver : DefaultPropertyResolver
    {
        private static readonly IPropertyResolver DefaultResolver = new DefaultPropertyResolver();

        protected override IEnumerable<PropertyInfo> FilterComplexTypes(IEnumerable<PropertyInfo> properties)
        {
            foreach (var propertyInfo in properties)
            {
                var type = propertyInfo.PropertyType;
                type = Nullable.GetUnderlyingType(type) ?? type;

                if (type.GetTypeInfo().IsPrimitive || type.GetTypeInfo().IsEnum || PrimitiveTypes.Contains(type))
                {
                    yield return propertyInfo;
                }
            }
        }

        public override IEnumerable<ColumnPropertyInfo> ResolveProperties(Type type)
        {
            if (FluentMap.FluentMapper.EntityMaps.TryGetValue(type, out IEntityMap entityMap))
            {
                foreach (var property in FilterComplexTypes(type.GetProperties()))
                {
                    var propertyMap = entityMap.PropertyMaps.FirstOrDefault(p => p.PropertyInfo.Name == property.Name);
                    if (propertyMap == null || !propertyMap.Ignored)
                    {
                        var fluentPropertyMap = propertyMap as DapperFluentPropertyMap;
                        if (fluentPropertyMap != null)
                        {
                            yield return new ColumnPropertyInfo(property, fluentPropertyMap.GeneratedOption ?? (fluentPropertyMap.Key ? DatabaseGeneratedOption.Identity : DatabaseGeneratedOption.None));
                        }
                        else
                        {
                            yield return new ColumnPropertyInfo(property);
                        }
                    }
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
