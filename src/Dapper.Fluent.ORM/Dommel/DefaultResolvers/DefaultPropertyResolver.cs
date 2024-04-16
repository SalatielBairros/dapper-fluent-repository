using Dapper.Fluent.ORM.Mapping;
using Dapper.FluentMap.Mapping;
using Dapper.FluentMap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Dapper.Fluent.ORM.Dommel;

/// <summary>
/// Default implemenation of the <see cref="IPropertyResolver"/> interface.
/// </summary>
public class DefaultPropertyResolver : IPropertyResolver
{
    private static readonly HashSet<Type> PrimitiveTypesSet = new()
    {
        typeof(object),
        typeof(string),
        typeof(Guid),
        typeof(decimal),
        typeof(double),
        typeof(float),
        typeof(DateTime),
        typeof(DateTimeOffset),
        typeof(TimeSpan),
        typeof(byte[]),
        typeof(DateOnly),
        typeof(TimeOnly),
    };

    /// <summary>
    /// Gets a collection of types that are considered 'primitive' for Dommel but are not for the CLR.
    /// Override this to specify your own set of types.
    /// </summary>
    protected virtual HashSet<Type> PrimitiveTypes => PrimitiveTypesSet;

    public virtual IEnumerable<ColumnPropertyInfo> ResolveProperties(Type type)
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
            throw new NotImplementedException("Entity must have a mapper associated");
        }
    }

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
}
