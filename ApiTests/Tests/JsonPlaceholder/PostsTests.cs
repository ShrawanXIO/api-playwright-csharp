using ApiTests.Models.JsonPlaceholder;
using ApiTests.Services.JsonPlaceholder;
using ApiTests.Tests;
using NUnit.Framework;

namespace ApiTests.Tests.JsonPlaceholder;

public class PostsTests : BaseApiTest
{
    protected override string BaseUrl => Settings.JsonPlaceholderBaseUrl;

    private PostsService _postsService = null!;

    [SetUp]
    public void SetupService()
    {
        _postsService = new PostsService(apiClient);
    }

    [Test]
    public async Task GetAllPosts_ReturnsPosts()
    {
        var result = await _postsService.GetAllAsync();

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public async Task GetPostById_ReturnsCorrectPost()
    {
        var result = await _postsService.GetByIdAsync(1);

        Assert.That(result.Id, Is.EqualTo(1));
    }
}