using ApiTests.Core;
using NUnit.Framework;

namespace ApiTests.Core;

public abstract class BaseApiTest
{
    protected static readonly ApiSettings Settings = ConfigLoader.Load();
    protected ApiClient apiClient { get; private set; } = null!;
    protected abstract string BaseUrl { get; }
    protected virtual IDictionary<string, string>? DefaultHeaders => null;
    [SetUp]
    public async Task Setup()
    {
        apiClient = new ApiClient();
        await apiClient.InitializeAsync(BaseUrl, DefaultHeaders);
    }

    [TearDown]
    public async Task TearDown()
    {
        await apiClient.DisposeAsync();
    }

}