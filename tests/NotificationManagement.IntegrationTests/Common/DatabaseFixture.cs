using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Xunit;

namespace NotificationManagement.IntegrationTests.Common;

// xUnit collection fixture: boots the shared factory once, then exposes a
// Reset() that wipes all table data (keeps schema) between individual tests.
public class DatabaseFixture : IAsyncLifetime
{
    private Respawner _respawner = null!;
    private NpgsqlConnection _connection = null!;

    public ApiWebApplicationFactory Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        _connection = new NpgsqlConnection(ApiWebApplicationFactory.TestConnectionString);
        await _connection.OpenAsync();

        // Single factory instance for the whole test run — reused by every
        // test class via dbFixture.Factory, so the app only boots once.
        // NOTE: we do NOT call MigrateAsync() here on purpose. Program.cs
        // already runs db.Database.Migrate() as part of its own startup,
        // which happens automatically the first time Factory.Services is
        // accessed below. Calling MigrateAsync() again here raced against
        // that first call and caused "relation already exists" errors.
        Factory = new ApiWebApplicationFactory();
        _ = Factory.Services; // forces host startup, which runs Program.cs's own migration

        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" },
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
        });
    }

    public async Task ResetAsync() => await _respawner.ResetAsync(_connection);

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
        Factory.Dispose();
    }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}