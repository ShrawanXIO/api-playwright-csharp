using ApiTests.Core;
using ApiTests.Models;
using ApiTests.Services;
using NUnit.Framework;

namespace ApiTests;

public class AuthTests
{
    private ApiClient _apiClient = null!;
    private AuthService _authService = null!;

    [SetUp]
    public async Task Setup()
    {
        _apiClient = new ApiClient();
        await _apiClient.InitializeAsync("https://dummyjson.com");
        _authService = new AuthService(_apiClient);
    }

    [Test]
    public async Task Login_WithValidCredentials_ReturnsAccessToken()
    {
        var request = new LoginRequest
        {
            Username = "emilys",
            Password = "emilyspass",
            ExpiresInMins = 30
        };

        var response = await _authService.LoginAsync(request);

        Assert.Multiple(() =>
        {
            Assert.That(response.AccessToken, Is.Not.Empty);
            Assert.That(response.RefreshToken, Is.Not.Empty);
            Assert.That(response.Username, Is.EqualTo("emilys"));
        });
    }

    [TearDown]
    public async Task TearDown()
    {
        await _apiClient.DisposeAsync();
    }
}