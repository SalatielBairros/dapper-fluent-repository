using Multiverse.Contracts;

namespace Multiverse.Tests;

[TestFixture]
public class GlobalSchemaProxyTests
{
    private IRepositorySettings _mockSettings;

    [SetUp]
    public void Setup()
    {
        // Create mock object for IRepositorySettings
        _mockSettings = Mock.Of<IRepositorySettings>();
    }

    [Test]
    public void GetSchema_ReturnsDefaultSchemaFromSettings()
    {
        // Arrange
        var expectedSchema = "mock_schema";
        Mock.Get(_mockSettings).Setup(s => s.DefaultSchema).Returns(expectedSchema);
        var schemaProxy = new GlobalSchemaProxy(_mockSettings);

        // Act
        var schema = schemaProxy.GetSchema();

        // Assert
        Assert.AreEqual(expectedSchema, schema);
    }
}
