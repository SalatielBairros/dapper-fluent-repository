using System;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;

namespace Dapper.Fluent.ORM.Mapping
{
    public class DapperFluentPropertyMap : DommelPropertyMap
    {
        public DapperFluentPropertyMap(PropertyInfo info)
            : base(info)
        {
        }

        public object DefaultValue { get; private set; }
        public int Lenght { get; private set; }
        public bool AllowNull { get; private set; } = true;

        public DapperFluentPropertyMap WithLenght(int lenght)
        {
            Lenght = lenght;
            return this;
        }

        public bool HasLenght() => Lenght > 0;
        public bool HasDefaultValue() => DefaultValue != null;
        public bool NotNull() => AllowNull = false;

        public DapperFluentPropertyMap Default(object defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }
    }
}
