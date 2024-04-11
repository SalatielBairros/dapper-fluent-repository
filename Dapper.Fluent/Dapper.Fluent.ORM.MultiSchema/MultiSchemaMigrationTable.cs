using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.MultiSchema;
using FluentMigrator.Runner.VersionTableInfo;

namespace Dapper.Fluent.ORM.Migrations
{
    [VersionTableMetaData]
    public class MultiSchemaMigrationTable : IVersionTableMetaData
    {
        private readonly ISchema _schema;

        public MultiSchemaMigrationTable(ISchema schema)
        {
            _schema = schema;
        }

        public object ApplicationContext { get; set; }

        public bool OwnsSchema => true;

        public string SchemaName => _schema.GetSchema();

        public string TableName => "migrations";

        public string ColumnName => "version";

        public string DescriptionColumnName => "description";

        public string UniqueIndexName => "id";

        public string AppliedOnColumnName => "appliedOn";
    }
}
