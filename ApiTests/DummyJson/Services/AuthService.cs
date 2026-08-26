using System.Text.Json;
using ApiTests.Core;
using ApiTests.DummyJson.Models;

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

}