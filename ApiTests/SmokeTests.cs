using ApiTests.Core;
using NUnit.Framework;

namespace ApiTests;

public class SmokeTests
{
    private ApiClient _apiClient = null!;

    [SetUp]
    public async Task Setup()
    {
        _apiClient = new ApiClient();
        await _apiClient.InitializeAsync("https://dummyjson.com");
    }

    [Test]
    public async Task GetProducts_ReturnsOk()
    {
        var response = await _apiClient.Context.GetAsync("/products");
        Assert.That(response.Ok, Is.True);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _apiClient.DisposeAsync();
    }
}