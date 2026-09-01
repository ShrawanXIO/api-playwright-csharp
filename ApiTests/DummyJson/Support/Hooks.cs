using ApiTests.Core;
using Reqnroll;

namespace ApiTests.DummyJson.Support;

[Binding]
public class Hooks
{
    private readonly ApiClient _apiClient;

    public Hooks(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        await _apiClient.InitializeAsync(ConfigLoader.Load().DummyJsonBaseUrl);
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        await _apiClient.DisposeAsync();
    }
}