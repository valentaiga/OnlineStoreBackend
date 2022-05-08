namespace OnlineStoreBackend.Abstractions.Models.Product;

public class ProductDto
{
    public string Id { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Path { get; set; }
    public string CategoryId { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ProductDto()
    {
    }

    public ProductDto WithId(string id)
    {
        Id = id;
        return this;
    }
}