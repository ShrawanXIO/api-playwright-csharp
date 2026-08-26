using ApiTests.Core;
using ApiTests.Models.DummyJson;
using ApiTests.Services.DummyJson;
using NUnit.Framework;

namespace ApiTests.Tests.DummyJson;

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

}