using Multiverse.Contracts;
using Npgsql;

namespace Multiverse.Postgres.Tests;

[TestFixture]
public class PostgresDapperConnectionTests
{
    private IRepositorySettings _mockSettings;
    private const string ConnectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;";

    [SetUp]
    public void Setup()
    {
        // Create mock object for IRepositorySettings
        _mockSettings = Mock.Of<IRepositorySettings>();
        Mock.Get(_mockSettings).Setup(s => s.ConnString).Returns(ConnectionString);
    }

    [Test]
    public void GetConnection_ReturnsNpgsqlConnectionWithCorrectConnectionString()
    {
        // Arrange
        var connection = new PostgresDapperConnection(_mockSettings);

        // Act
        var dbConnection = connection.GetConnection() as NpgsqlConnection;

        // Assert
        Assert.That(dbConnection, Is.Not.Null);
        Assert.That(dbConnection.ConnectionString, Is.EqualTo(ConnectionString));
    }
}
