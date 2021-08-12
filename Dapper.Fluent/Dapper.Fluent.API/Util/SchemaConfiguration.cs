using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Fluent.Infra.Contracts;

namespace Dapper.Fluent.API.Util
{
    public class SchemaConfiguration : ISchemaConfiguration
    {
        public string Schema => "dapper";
    }
}
