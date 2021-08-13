using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Fluent.ORM.Contracts;
using FluentMigrator.Runner;

namespace Dapper.Fluent.ORM
{
    public class DapperRepositoryRunner : IDapperORMRunner
    {
        private readonly IMigrationRunner _migrationRunner;
        private readonly IMapperConfiguration _mappers;

        public DapperRepositoryRunner(IMigrationRunner migrationRunner, IMapperConfiguration mappers)
        {
            this._migrationRunner = migrationRunner;
            this._mappers = mappers;
        }
        public void AddMapsAndRunMigrations()
        {
            _mappers.ConfigureMappers();
            _migrationRunner.MigrateUp();
        }
    }
}
