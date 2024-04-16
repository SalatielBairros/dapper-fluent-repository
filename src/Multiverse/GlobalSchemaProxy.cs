using Multiverse.Contracts;

namespace Multiverse;

public class GlobalSchemaProxy : ISchema
{
    private readonly IRepositorySettings _settings;
    public GlobalSchemaProxy(IRepositorySettings settings)
    {
        _settings = settings;
    }

    public string GetSchema()
    {
        return _settings.DefaultSchema;
    }
}