namespace ApiTests.Models;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

public class ProductListResponse
{
    public List<Product> Products { get; set; } = new();
    public int Total { get; set; }
    public int Skip { get; set; }
    public int Limit { get; set; }
}

public class CreateProductRequest
{
    public string Title { get; set; } = string.Empty;
}

public class UpdateProductRequest
{
    public string Title { get; set; } = string.Empty;
}

public class DeleteProductResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTime DeletedOn { get; set; }
}