using Multiverse.Contracts;
using Multiverse.Dommel;

namespace Multiverse.Postgres.Tests;

[TestFixture]
public class PostgresRepositoryTests
{
    private IRepositorySettings _mockSettings;
    private IDapperORMRunner _mockRunner;
    private ITableNameResolver _mockTableNameResolver;

    [SetUp]
    public void Setup()
    {        
        _mockSettings = Mock.Of<IRepositorySettings>();
        _mockRunner = Mock.Of<IDapperORMRunner>();
        _mockTableNameResolver = Mock.Of<ITableNameResolver>();
    }

    [Test]
    public void ConfigureDatabase_EnablesLegacyTimestampBehavior()
    {
        // Arrange
        var repository = new PostgresRepository<object>(_mockSettings, _mockRunner, _mockTableNameResolver);

        // Act
        repository.ConfigureDatabase();

        // Assert
        Assert.Multiple(() =>
        {            
            Assert.That(AppContext.TryGetSwitch("Npgsql.EnableLegacyTimestampBehavior", out bool enabled), Is.True);
            Assert.That(enabled, Is.True);
        });
    }
}
