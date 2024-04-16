using System;

namespace Dapper.Fluent.ORM.Contracts;

public interface IJsonPropertyHandler
{
    void SetJsonTypes(Type[] types);
}