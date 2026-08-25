using System.Text.Json;
using ApiTests.Core;
using ApiTests.Models;

namespace ApiTests.Services;
public class AuthService
{
    private readonly ApiClient _apiClient;

    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public AuthService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var response = await _apiClient.Context.PostAsync("/auth/login", new()
        {
            DataObject = request
        });

        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<LoginResponse>(body, _jsonOptions)!;
    }

}