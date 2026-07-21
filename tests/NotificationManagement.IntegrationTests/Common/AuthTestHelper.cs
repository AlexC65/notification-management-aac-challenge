using System.Net.Http.Json;
using System.Text.Json;

namespace NotificationManagement.IntegrationTests.Common;

// Registers a fresh user via the real API and logs in to obtain a JWT,
// so integration tests can call protected endpoints without hand-crafting
// a token. Assumes:
//   POST /api/auth/register  { email, name, password }
//   POST /api/auth/login     { email, password } -> { token: "..." }
// Adjust field/property names below if your DTOs differ.
public static class AuthTestHelper
{
    public static async Task<string> RegisterAndLoginAsync(
        HttpClient client,
        string? email = null,
        string password = "Test123!")
    {
        email ??= $"test_{Guid.NewGuid():N}@example.com";

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", new
        {
            email,
            name = "Test User",
            password
        });

        if (!registerResponse.IsSuccessStatusCode)
        {
            var body = await registerResponse.Content.ReadAsStringAsync();
            throw new InvalidOperationException(
                $"Register failed with {registerResponse.StatusCode}: {body}");
        }

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email,
            password
        });

        if (!loginResponse.IsSuccessStatusCode)
        {
            var body = await loginResponse.Content.ReadAsStringAsync();
            throw new InvalidOperationException(
                $"Login failed with {loginResponse.StatusCode}: {body}");
        }

        var json = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();

        // Try common property names in case yours differs from "token".
        if (json.TryGetProperty("token", out var tokenProp))
            return tokenProp.GetString()!;
        if (json.TryGetProperty("accessToken", out var accessTokenProp))
            return accessTokenProp.GetString()!;

        throw new InvalidOperationException(
            $"Could not find a token/accessToken property in login response: {json}");
    }
}