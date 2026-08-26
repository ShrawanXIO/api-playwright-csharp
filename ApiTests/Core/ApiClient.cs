using Microsoft.Playwright;

namespace ApiTests.Core;
public class ApiClient
{
    private IPlaywright _playwright = null!;
    public IAPIRequestContext Context { get; private set; } = null!;

   public async Task InitializeAsync(string baseUrl, IDictionary<string, string>? extraHeaders = null)
{
    _playwright = await Playwright.CreateAsync();
    Context = await _playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
    {
        BaseURL = baseUrl,
        ExtraHTTPHeaders = extraHeaders
    });
}

    public async Task DisposeAsync()
    {
        await Context.DisposeAsync();
        _playwright.Dispose();
    }
}