using Dapper.Fluent.ORM.MultiSchema;
using FluentMigrator.Runner.VersionTableInfo;

namespace Dapper.Fluent.ORM.Migrations
{
    [VersionTableMetaData]
    public class MultiSchemaMigrationTable : IVersionTableMetaData
    {
        private readonly IRequestInfo _info;

        public MultiSchemaMigrationTable(IRequestInfo info)
        {
            _info = info;
        }

        public object ApplicationContext { get; set; }

        public bool OwnsSchema => true;

        public string SchemaName => _info.GetSchema();

        public string TableName => "migrations";

        public string ColumnName => "version";

        public string DescriptionColumnName => "description";

        public string UniqueIndexName => "id";

        public string AppliedOnColumnName => "appliedOn";
    }
}
