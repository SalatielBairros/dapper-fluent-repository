namespace Multiverse.Postgres.Tests;

public class PostgresRepositorySettingsTests
{
    [Test]
    public void Should_DefaultSchemaBePublic()
    {
        var settings = new PostgresRepositorySettings();
        Assert.That(settings.DefaultSchema, Is.EqualTo("public"));
    }
}
