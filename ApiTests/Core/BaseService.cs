using System.Text.Json;

namespace ApiTests.Core;

public abstract class BaseService
{
    protected readonly ApiClient ApiClient;

    protected static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    protected BaseService(ApiClient apiClient)
    {
        ApiClient = apiClient;
    }
}