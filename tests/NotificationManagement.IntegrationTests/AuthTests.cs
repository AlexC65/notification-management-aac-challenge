
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NotificationManagement.IntegrationTests.Common;

namespace NotificationManagement.IntegrationTests;

[Collection("Database collection")]
public class AuthTests : IAsyncLifetime
{
    private readonly DatabaseFixture _dbFixture;
    private readonly HttpClient _client;

    public Task DisposeAsync()
    {
        _client.Dispose();
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        return _dbFixture.ResetAsync();
    }

    public AuthTests(DatabaseFixture dbFixture)
    {
        _dbFixture = dbFixture;
        _client = _dbFixture.Factory.CreateClient();
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ReturnsEmailAlreadyExists()
    {
        var payloadA = new
        {
            email = "usera@test.com",
            name = "userA",
            password = "12345678"
        };

        var registerResponseA = await _client.PostAsJsonAsync("/api/auth/register", payloadA);
        registerResponseA.StatusCode.Should().Be(HttpStatusCode.OK);

        var payloadB = new
        {
            email = "usera@test.com",
            name = "userB",
            password = "12345678"
        };

        var registerResponseB = await _client.PostAsJsonAsync("/api/auth/register", payloadB);
        registerResponseB.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ReturnsUnauthorized()
    {
        var payloadCreate = new
        {
            email = "usera@test.com",
            name = "userA",
            password = "12345678"
        };

        var registerResponseA = await _client.PostAsJsonAsync("/api/auth/register", payloadCreate);
        registerResponseA.StatusCode.Should().Be(HttpStatusCode.OK);

        var payloadLogin = new
        {
            email = "usera@test.com",
            password = "abcdefgh"
        };

        var loginResponseA = await _client.PostAsJsonAsync("/api/auth/login", payloadLogin);
        loginResponseA.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}