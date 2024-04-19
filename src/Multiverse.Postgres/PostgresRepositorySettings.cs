using Multiverse.Contracts;

namespace Multiverse.Postgres;

public class PostgresRepositorySettings : IRepositorySettings
{
    public PostgresRepositorySettings()
    {
        DefaultSchema = "public";
    }

    public string ConnString { get; set; }
    public string DefaultSchema { get; set; }
    public bool AutomaticMigrationsEnabled { get; set; }
}
