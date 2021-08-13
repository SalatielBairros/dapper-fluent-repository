using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Fluent.ORM.Contracts;

namespace Dapper.Fluent.ORM.Postgres
{
    public class PostgresRepositorySettings : IRepositorySettings
    {
        public PostgresRepositorySettings()
        {
            DefaultSchema = "public";
        }

        public string ConnString { get; set; }
        public string Schema { get; set; }
        public string DefaultSchema { get; set; }
    }
}
