    using Multiverse.Contracts;
using Multiverse.Repository;
using System.Data;
using System;
using System.Threading.Tasks;

namespace Multiverse.Tests.Repository;

[TestFixture]
public class DapperConnectionTests
{
    private Mock<IRepositorySettings> _mockSettings;
    private DapperConnection<FakeDbConnection> _dapperConnection;

    [SetUp]
    public void SetUp()
    {
        _mockSettings = new Mock<IRepositorySettings>();
        _mockSettings.Setup(s => s.ConnString).Returns("FakeConnectionString");

        _dapperConnection = new DapperConnection<FakeDbConnection>(_mockSettings.Object);
    }

    [Test]
    public void Use_Action_CallsActionWithConnection()
    {
        // Arrange
        bool actionCalled = false;
        Action<IDbConnection> action = conn => actionCalled = true;

        // Act
        _dapperConnection.Use(action);

        // Assert
        Assert.IsTrue(actionCalled);
    }

    [Test]
    public void Use_Func_CallsFuncWithConnection()
    {
        // Arrange
        Func<IDbConnection, int> func = conn => 42;

        // Act
        var result = _dapperConnection.Use(func);

        // Assert
        Assert.AreEqual(42, result);
    }

    [Test]
    public async Task UseAsync_Action_CallsActionWithConnectionAsync()
    {
        // Arrange
        bool actionCalled = false;
        Func<IDbConnection, Task> action = async conn =>
        {
            await Task.Delay(10); 
            actionCalled = true;
        };

        // Act
        await _dapperConnection.UseAsync(action);

        // Assert
        Assert.IsTrue(actionCalled);
    }

    [Test]
    public async Task UseAsync_Func_ReturnsResultFromFuncAsync()
    {
        // Arrange
        Func<IDbConnection, Task<int>> func = async conn =>
        {
            await Task.Delay(10); 
            return 42;
        };

        // Act
        var result = await _dapperConnection.UseAsync(func);

        // Assert
        Assert.AreEqual(42, result);
    }
}