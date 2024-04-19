using Multiverse.Contracts;

namespace Multiverse.MultiSchema;

public class Schema : ISchema
{
    private readonly string _schema;

    public Schema(string schema)
    {
        this._schema = schema;
    }
    public string GetSchema() => _schema;
}