using System.Text.Json;
using ApiTests.Core;
using ApiTests.JsonPlaceholder.Models;

namespace ApiTests.JsonPlaceholder.Services;

public class PostsService
{
    private readonly ApiClient _apiClient;

    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public PostsService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<Post>> GetAllAsync()
    {
        var response = await _apiClient.Context.GetAsync("/posts");
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<List<Post>>(body, _jsonOptions)!;
    }

    public async Task<Post> GetByIdAsync(int id)
    {
        var response = await _apiClient.Context.GetAsync($"/posts/{id}");
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<Post>(body, _jsonOptions)!;
    }
}