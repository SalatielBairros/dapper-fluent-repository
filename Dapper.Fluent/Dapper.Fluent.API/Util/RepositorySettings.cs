using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Fluent.ORM.Contracts;
using Microsoft.Extensions.Configuration;

namespace Dapper.Fluent.API.Util
{
    public class RepositorySettings : IRepositorySettings
    {
        public RepositorySettings(IConfiguration configuration)
        {
            ConnString = configuration["ConnectionString"];
        }

        public string Schema => "dapper";

        public string ConnString { get; }
    }
}
