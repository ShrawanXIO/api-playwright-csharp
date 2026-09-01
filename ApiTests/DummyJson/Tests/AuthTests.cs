using ApiTests.Core;
using ApiTests.DummyJson.Models;
using ApiTests.DummyJson.Services;
using NUnit.Framework;

namespace ApiTests.DummyJson.Tests;

public class AuthTests : BaseApiTest
{
    private AuthService _authService = null!;

    protected override string BaseUrl => Settings.DummyJsonBaseUrl;

    [SetUp]
    public async Task SetupService()
    {
        _authService = new AuthService(apiClient);
    }

    [Test]
    public async Task Login_WithValidCredentials_ReturnsAccessToken()
    {
        var request = new LoginRequest
        {
            Username = Settings.DummyJsonUsername,
            Password = Settings.DummyJsonPassword,
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

    [Test]
    public async Task Login_WithExpiresInMins30_TokenExpiresInApproximately30Minutes()
    {
        var request = new LoginRequest
        {
            Username = Settings.DummyJsonUsername,
            Password = Settings.DummyJsonPassword,
            ExpiresInMins = 30
        };

        var response = await _authService.LoginAsync(request);

        var expiry = JwtHelper.GetExpiry(response.AccessToken);
        var expectedExpiry = DateTime.UtcNow.AddMinutes(30);

        Assert.That(expiry, Is.EqualTo(expectedExpiry).Within(TimeSpan.FromMinutes(1)));
    }

}