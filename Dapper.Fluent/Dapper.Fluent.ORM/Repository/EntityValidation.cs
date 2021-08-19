using System.Linq;
using Dapper.Fluent.Mapping;
using Dapper.Fluent.ORM.Mapping;
using System;

namespace Dapper.Fluent.ORM.Repository
{
    public static class EntityValidation
    {
        public static void ThrowIfErrorOn<T>(T entity) where T : class
        {
            var map = FluentMapping.GetMapOf<T>();
            if (map != null && map.IsValidated)
            {
                map.PropertyMaps.Cast<DapperFluentPropertyMap>().ToList()
                    .ForEach(p =>
                    {
                        var type = p.PropertyInfo.PropertyType;

                        NullValidation(entity, p);
                        LengthValidation(entity, p, type);
                    });
            }
        }

        private static void LengthValidation<T>(T entity, DapperFluentPropertyMap p, Type type) where T : class
        {
            if (type == typeof(string) && p.HasLenght())
            {
                var value = typeof(T).GetProperty(p.PropertyInfo.Name).GetValue(entity).ToString();
                if (value.Length > p.Lenght)
                {
                    throw new InvalidOperationException($"The property {p.PropertyInfo.Name} is larger than {p.Lenght}.");
                }
            }
        }

        private static void NullValidation<T>(T entity, DapperFluentPropertyMap p) where T : class
        {
            if (!p.AllowNull && !p.HasDefaultValue())
            {
                var value = typeof(T).GetProperty(p.PropertyInfo.Name).GetValue(entity);
                if (value == null)
                {
                    throw new InvalidOperationException($"The property {p.PropertyInfo.Name} should not be null.");
                }
            }
        }
    }
}
