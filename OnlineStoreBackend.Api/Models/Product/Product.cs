using OnlineStoreBackend.Abstractions.Models.Product;

namespace OnlineStoreBackend.Api.Models.Product;

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public string Path { get; set; }
    public string CategoryId { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Product()
    {
    }

    public Product(ProductDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        Code = dto.Code;
        IsActive = dto.IsActive;
        Description = dto.Description;
        Path = dto.Path;
        CategoryId = dto.CategoryId;
        UpdatedAt = dto.UpdatedAt;
    }
}