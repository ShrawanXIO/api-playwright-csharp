using System.Text.Json;
using ApiTests.Core;
using ApiTests.DummyJson.Models;
using Microsoft.Playwright;

namespace ApiTests.DummyJson.Services;

public class AuthService : BaseService
{

    public AuthService(ApiClient apiClient) : base(apiClient)
    {
    }
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var response = await ApiClient.Context.PostAsync("/auth/login", new()
        {
            DataObject = request
        });

        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<LoginResponse>(body, JsonOptions)!;
    }

    public async Task<IAPIResponse> GetCurrentUserAsync(string accessToken)
    {
        return await ApiClient.Context.GetAsync("/auth/me", new()
        {
            Headers = new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {accessToken}" }
        }
        });
    }

    public async Task<RefreshResponse> RefreshAsync(RefreshRequest request)
    {
        var response = await ApiClient.Context.PostAsync("/auth/refresh", new()
        {
            DataObject = request
        });

        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<RefreshResponse>(body, JsonOptions)!;
    }

}