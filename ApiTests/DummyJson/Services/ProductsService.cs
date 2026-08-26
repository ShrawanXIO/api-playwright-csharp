using System.Text.Json;
using ApiTests.Core;
using ApiTests.DummyJson.Models;

namespace ApiTests.DummyJson.Services;

public class ProductsService : BaseService
{
    public ProductsService(ApiClient apiClient) : base(apiClient)
    {
    }
    public async Task<ProductListResponse> GetAllAsync()
    {
        var response = await ApiClient.Context.GetAsync("/products");
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<ProductListResponse>(body, JsonOptions)!;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var response = await ApiClient.Context.GetAsync($"/products/{id}");
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<Product>(body, JsonOptions)!;
    }

    public async Task<Product> CreateAsync(CreateProductRequest request)
    {
        var response = await ApiClient.Context.PostAsync("/products/add", new()
        {
            DataObject = request
        });
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<Product>(body, JsonOptions)!;
    }

    public async Task<Product> UpdateAsync(int id, UpdateProductRequest request)
    {
        var response = await ApiClient.Context.PutAsync($"/products/{id}", new()
        {
            DataObject = request
        });
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<Product>(body, JsonOptions)!;
    }

    public async Task<DeleteProductResponse> DeleteAsync(int id)
    {
        var response = await ApiClient.Context.DeleteAsync($"/products/{id}");
        var body = await response.TextAsync();
        return JsonSerializer.Deserialize<DeleteProductResponse>(body, JsonOptions)!;
    }
}