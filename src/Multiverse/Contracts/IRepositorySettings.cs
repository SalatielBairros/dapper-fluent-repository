
namespace Multiverse.Contracts;

public interface IRepositorySettings
{
    string ConnString { get; }
    string DefaultSchema { get; }
    bool AutomaticMigrationsEnabled { get; }
}
