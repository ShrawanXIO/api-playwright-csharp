using ApiTests.Core;
using ApiTests.DummyJson.Models;
using ApiTests.DummyJson.Services;
using NUnit.Framework;

namespace ApiTests.DummyJson.Tests;

public class ProductsTests : BaseApiTest
{
  
    private ProductsService _productsService = null!;

    protected override string BaseUrl => Settings.DummyJsonBaseUrl;

    [SetUp]
    public async Task SetupService()
    {
        _productsService = new ProductsService(apiClient);
    }

    [Test]
    public async Task GetAllProducts_ReturnsProducts()
    {
        var result = await _productsService.GetAllAsync();

        Assert.Multiple(() =>
        {
            Assert.That(result.Products, Is.Not.Empty);
            Assert.That(result.Total, Is.GreaterThan(0));
        });
    }

    [Test]
    public async Task GetProductById_ReturnsCorrectProduct()
    {
        var result = await _productsService.GetByIdAsync(1);

        Assert.That(result.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task CreateProduct_ReturnsNewProductWithId()
    {
        var request = new CreateProductRequest { Title = "Test Product" };

        var result = await _productsService.CreateAsync(request);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.GreaterThan(0));
            Assert.That(result.Title, Is.EqualTo("Test Product"));
        });
    }

    [Test]
    public async Task UpdateProduct_ReturnsUpdatedTitle()
    {
        var request = new UpdateProductRequest { Title = "Updated Product Title" };

        var result = await _productsService.UpdateAsync(1, request);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo("Updated Product Title"));
        });
    }

    [Test]
    public async Task DeleteProduct_ReturnsIsDeletedTrue()
    {
        var result = await _productsService.DeleteAsync(1);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.IsDeleted, Is.True);
        });
    }
   
}