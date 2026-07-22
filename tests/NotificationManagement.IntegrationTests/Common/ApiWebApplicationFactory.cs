using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NotificationManagement.IntegrationTests.Common;

// Boots the real API in-memory (Program.cs) but pointed at postgres-test
// instead of postgres_dev. Program.cs must exist and be public/internal
// visible via <InternalsVisibleTo> or a partial "public partial class Program".
public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string TestConnectionString =
        "Host=localhost;Port=5433;Database=notifications_test;Username=postgres;Password=postgres";

    public ApiWebApplicationFactory()
    {
        // Set as environment variables (not just via ConfigureAppConfiguration)
        // because Program.cs reads some of these directly from
        // builder.Configuration BEFORE builder.Build() runs. Environment
        // variables are the one config source guaranteed to be loaded from
        // the very start of WebApplication.CreateBuilder(args), unlike our
        // ConfigureWebHost customizations below, which apply later in the
        // pipeline (too late for code that runs between CreateBuilder and
        // Build in Program.cs).
        // Double underscore (__) is the env-var convention for the ':'
        // section separator used in .NET configuration keys.
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", TestConnectionString);
        Environment.SetEnvironmentVariable("Jwt__Secret", "this-is-a-test-only-secret-key-min-32-chars-long");
        Environment.SetEnvironmentVariable("Jwt__Issuer", "NotificationManagement.Tests");
        Environment.SetEnvironmentVariable("Jwt__Audience", "NotificationManagement.Tests");
        Environment.SetEnvironmentVariable("Jwt__ExpirationHours", "1");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        // Belt-and-suspenders: also override via ConfigureAppConfiguration
        // for anything read lazily later (e.g. via IOptions<T> resolved from
        // DI), in addition to the environment variables set in the
        // constructor above.
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = TestConnectionString,
                ["Jwt:Secret"] = "this-is-a-test-only-secret-key-min-32-chars-long",
                ["Jwt:Issuer"] = "NotificationManagement.Tests",
                ["Jwt:Audience"] = "NotificationManagement.Tests",
                ["Jwt:ExpirationHours"] = "1"
            });
        });

                // Avoid Windows Event Log as a logging provider during tests.
        // On some machines the RPC service hiccups momentarily, which makes
        // EventLogLogger throw and MASKS the real underlying exception
        // (e.g. a DB connection error) behind a confusing logging error
        // instead. Console-only logging is reliable and enough for tests.
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
    }
}