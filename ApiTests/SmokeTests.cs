using Microsoft.Playwright;
using NUnit.Framework;

namespace ApiTests;
public class Smoketests
{
    private IPlaywright _playwright;
    private IAPIRequestContext _apiContext;

    [SetUp]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _apiContext = await _playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
             BaseURL = "https://dummyjson.com"         
        });
    }

    [Test]
    public async Task GetProducts_ReturnsOk()
    {
        var response = await _apiContext.GetAsync("/products");
        Assert.That(response.Ok, Is.True);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _apiContext.DisposeAsync();
        _playwright.Dispose();
    }
}

