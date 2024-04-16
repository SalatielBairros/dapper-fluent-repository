using System.Data;
using System;
using Newtonsoft.Json;

namespace Dapper.Fluent.ORM.Mapping;

public class DefaultJsonTypeHandler : SqlMapper.ITypeHandler
{
    public void SetValue(IDbDataParameter parameter, object value)
    {
        parameter.Value = value;
    }

    public object Parse(Type destinationType, object value)
    {
        return JsonConvert.DeserializeObject(value as string, destinationType);
    }
}