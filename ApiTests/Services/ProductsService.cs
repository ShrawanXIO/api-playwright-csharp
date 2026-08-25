using System.Text.Json;
using ApiTests.Core;
using ApiTests.Models;

namespace ApiTests.Services;

public class ProductsService
{
    private readonly ApiClient _apiClient;

    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public ProductsService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<ProductListResponse> GetAllAsync()
    {
        var response = await _apiClient.Context.GetAsync("/products");
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<ProductListResponse>(body, _jsonOptions)!;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var response = await _apiClient.Context.GetAsync($"/products/{id}");
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<Product>(body, _jsonOptions)!;
    }

    public async Task<Product> CreateAsync(CreateProductRequest request)
    {
        var response = await _apiClient.Context.PostAsync("/products/add", new()
        {
            DataObject = request
        });
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<Product>(body, _jsonOptions)!;
    }

    public async Task<Product> UpdateAsync(int id, UpdateProductRequest request)
    {
        var response = await _apiClient.Context.PutAsync($"/products/{id}", new()
        {
            DataObject = request
        });
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<Product>(body, _jsonOptions)!;
    }

    public async Task<DeleteProductResponse> DeleteAsync(int id)
    {
        var response = await _apiClient.Context.DeleteAsync($"/products/{id}");
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<DeleteProductResponse>(body, _jsonOptions)!;
    }
}