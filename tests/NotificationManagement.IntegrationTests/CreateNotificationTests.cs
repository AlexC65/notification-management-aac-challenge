using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using NotificationManagement.IntegrationTests.Common;
using Xunit;

namespace NotificationManagement.IntegrationTests;

[Collection("Database collection")]
public class CreateNotificationTests : IAsyncLifetime
{
    private readonly DatabaseFixture _dbFixture;
    private readonly HttpClient _client;

    public CreateNotificationTests(DatabaseFixture dbFixture)
    {
        _dbFixture = dbFixture;
        _client = _dbFixture.Factory.CreateClient();
    }

    public Task InitializeAsync() => _dbFixture.ResetAsync();

    public Task DisposeAsync()
    {
        _client.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task CreateAsync_WithValidTokenAndData_ReturnsCreatedWithSequenceNumber()
    {
        var token = await AuthTestHelper.RegisterAndLoginAsync(_client);
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var payload = new
        {
            title = "Welcome",
            content = "Thanks for signing up!",
            channel = "Email",
            recipient = "someone@example.com"
        };

        var response = await _client.PostAsJsonAsync("/api/notifications", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateAsync_WithoutToken_ReturnsUnauthorized()
    {
        var payload = new
        {
            title = "Welcome",
            content = "Thanks for signing up!",
            channel = "Email",
            recipient = "someone@example.com"
        };

        var response = await _client.PostAsJsonAsync("/api/notifications", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidChannel_ReturnsBadRequest()
    {
        var token = await AuthTestHelper.RegisterAndLoginAsync(_client);
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // "Fax" isn't a valid ChannelType value (Email/Sms/Push), so with
        // JsonStringEnumConverter this fails model binding before it ever
        // reaches the controller action.
        var payload = new
        {
            title = "Welcome",
            content = "Thanks for signing up!",
            channel = "Fax",
            recipient = "someone@example.com"
        };

        var response = await _client.PostAsJsonAsync("/api/notifications", payload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetMyNotifications_WithValidToken_ReturnsOnlyNotificationsOwnedByCaller()
    {
        var clientA = _dbFixture.Factory.CreateClient();
        var clientB = _dbFixture.Factory.CreateClient();

        var tokenA = await AuthTestHelper.RegisterAndLoginAsync(clientA);
        var tokenB = await AuthTestHelper.RegisterAndLoginAsync(clientB);

        clientA.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenA);
        clientB.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenB);

        var payloadA = new
        {
            title = "Welcome A",
            content = "User A Notification",
            channel = "Email",
            recipient = "someone@example.com"
        };

        var payloadB = new
        {
            title = "Welcome B",
            content = "User B Notification",
            channel = "Email",
            recipient = "someone@example.com"
        };

        var responseA = await clientA.PostAsJsonAsync("/api/notifications", payloadA);
        var responseB = await clientB.PostAsJsonAsync("/api/notifications", payloadB);

        responseA.StatusCode.Should().Be(HttpStatusCode.Created);
        responseB.StatusCode.Should().Be(HttpStatusCode.Created);

        var getResponse = await clientA.GetAsync("/api/notifications?page=1&pageSize=20");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var notifications = await getResponse.Content.ReadFromJsonAsync<JsonElement>();

        notifications.GetArrayLength().Should().Be(1);
        notifications[0].GetProperty("title").GetString().Should().Be("Welcome A");

        clientA.Dispose();
        clientB.Dispose();

    }

    [Fact]
    public async Task Delete_ByNonOwner_ReturnsForbidden()
    {
        var clientA = _dbFixture.Factory.CreateClient();
        var clientB = _dbFixture.Factory.CreateClient();

        var tokenA = await AuthTestHelper.RegisterAndLoginAsync(clientA);
        var tokenB = await AuthTestHelper.RegisterAndLoginAsync(clientB);

        clientA.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenA);
        clientB.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenB);

        var payloadA = new
        {
            title = "Welcome A",
            content = "User A Notification",
            channel = "Email",
            recipient = "someone@example.com"
        };

        var responseA = await clientA.PostAsJsonAsync("/api/notifications", payloadA);

        responseA.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdA = await responseA.Content.ReadFromJsonAsync<JsonElement>();

        int  notificationIdA = createdA.GetProperty("notificationId").GetInt32();

/* Console.WriteLine($"notificationIdA = {notificationIdA}");
notificationIdA.Should().Be(-999); */

        // User B tries to delete User A's notification by its sequence number.
        var deletResponseB = await clientB.DeleteAsync($"/api/notifications/{notificationIdA}");

        deletResponseB.StatusCode.Should().Be(HttpStatusCode.NotFound);

        clientA.Dispose();
        clientB.Dispose();
    }
}