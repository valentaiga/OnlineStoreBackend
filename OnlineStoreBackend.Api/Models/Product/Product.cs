using OnlineStoreBackend.Abstractions.Models.Product;

namespace OnlineStoreBackend.Api.Models.Product;

public class Product
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Path { get; set; }
    public string CategoryId { get; set; }

    public Product()
    {
    }

    public Product(ProductDto dto)
    {
        Name = dto.Name;
        IsActive = dto.IsActive;
        Path = dto.Path;
        CategoryId = dto.CategoryId;
    }
}