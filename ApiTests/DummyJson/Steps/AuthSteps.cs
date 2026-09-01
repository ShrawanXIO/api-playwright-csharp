using ApiTests.Core;
using ApiTests.DummyJson.Models;
using ApiTests.DummyJson.Services;
using Reqnroll;

namespace ApiTests.DummyJson.Steps;

[Binding]
public class AuthSteps
{
    private readonly AuthService _authService;
    private LoginRequest _loginRequest = null!;
    private LoginResponse _loginResponse = null!;

    public AuthSteps(AuthService authService)
    {
        _authService = authService;
    }

    [Given("I have valid DummyJSON credentials")]
    public void GivenIHaveValidDummyJsonCredentials()
    {
        var settings = ConfigLoader.Load();
        _loginRequest = new LoginRequest
        {
            Username = settings.DummyJsonUsername,
            Password = settings.DummyJsonPassword,
            ExpiresInMins = 30
        };
    }

    [When("I log in")]
    public async Task WhenILogIn()
    {
        _loginResponse = await _authService.LoginAsync(_loginRequest);
    }

    [Then("I should receive a valid access token")]
    public void ThenIShouldReceiveAValidAccessToken()
    {
        Assert.That(_loginResponse.AccessToken, Is.Not.Empty);
    }
}