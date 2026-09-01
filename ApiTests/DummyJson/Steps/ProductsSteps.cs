using ApiTests.DummyJson.Models;
using ApiTests.DummyJson.Services;
using Reqnroll;

namespace ApiTests.DummyJson.Steps;

[Binding]
public class ProductsSteps
{
    private readonly ProductsService _productsService;

    private int _productId;
    private CreateProductRequest _createRequest = null!;
    private UpdateProductRequest _updateRequest = null!;
    private ProductListResponse _listResult = null!;
    private Product _productResult = null!;
    private DeleteProductResponse _deleteResult = null!;

    public ProductsSteps(ProductsService productsService)
    {
        _productsService = productsService;
    }

    [When("I request all products")]
    public async Task WhenIRequestAllProducts()
    {
        _listResult = await _productsService.GetAllAsync();
    }

    [Then("I should receive a list of products")]
    public void ThenIShouldReceiveAListOfProducts()
    {
        Assert.That(_listResult.Products, Is.Not.Empty);
    }

    [When("I request the product with id {int}")]
    public async Task WhenIRequestTheProductWithId(int id)
    {
        _productId = id;
        _productResult = await _productsService.GetByIdAsync(id);
    }

    [Then("I should receive that product")]
    public void ThenIShouldReceiveThatProduct()
    {
        Assert.That(_productResult.Id, Is.EqualTo(_productId));
    }

    [Given("I have a new product titled {string}")]
    public void GivenIHaveANewProductTitled(string title)
    {
        _createRequest = new CreateProductRequest { Title = title };
    }

    [When("I create the product")]
    public async Task WhenICreateTheProduct()
    {
        _productResult = await _productsService.CreateAsync(_createRequest);
    }

    [Then("the created product should have a title of {string}")]
    public void ThenTheCreatedProductShouldHaveATitleOf(string expectedTitle)
    {
        Assert.That(_productResult.Title, Is.EqualTo(expectedTitle));
    }

    [Given("I want to update product {int} with the title {string}")]
    public void GivenIWantToUpdateProductWithTheTitle(int id, string title)
    {
        _productId = id;
        _updateRequest = new UpdateProductRequest { Title = title };
    }

    [When("I update the product")]
    public async Task WhenIUpdateTheProduct()
    {
        _productResult = await _productsService.UpdateAsync(_productId, _updateRequest);
    }

    [Then("the updated product should have the title {string}")]
    public void ThenTheUpdatedProductShouldHaveTheTitle(string expectedTitle)
    {
        Assert.That(_productResult.Title, Is.EqualTo(expectedTitle));
    }

    [When("I delete the product with id {int}")]
    public async Task WhenIDeleteTheProductWithId(int id)
    {
        _deleteResult = await _productsService.DeleteAsync(id);
    }

    [Then("the product should be marked as deleted")]
    public void ThenTheProductShouldBeMarkedAsDeleted()
    {
        Assert.That(_deleteResult.IsDeleted, Is.True);
    }
}