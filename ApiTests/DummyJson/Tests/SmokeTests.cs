using ApiTests.Core;
using NUnit.Framework;

namespace ApiTests.DummyJson.Tests;

public class SmokeTests : BaseApiTest
{
    protected override string BaseUrl => Settings.DummyJsonBaseUrl;

    [Test]
    public async Task GetProducts_ReturnsOk()
    {
        var response = await apiClient.Context.GetAsync("/products");
        Assert.That(response.Ok, Is.True);
    }

}