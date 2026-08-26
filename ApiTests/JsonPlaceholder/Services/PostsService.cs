using System.Text.Json;
using ApiTests.Core;
using ApiTests.JsonPlaceholder.Models;

namespace ApiTests.JsonPlaceholder.Services;

public class PostsService : BaseService
{

    public PostsService(ApiClient apiClient) : base(apiClient)
    {
    }

    public async Task<List<Post>> GetAllAsync()
    {
        var response = await ApiClient.Context.GetAsync("/posts");
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<List<Post>>(body, JsonOptions)!;
    }

    public async Task<Post> GetByIdAsync(int id)
    {
        var response = await ApiClient.Context.GetAsync($"/posts/{id}");
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<Post>(body, JsonOptions)!;
    }
}