namespace OnlineStoreBackend.Api.Models.Product;

public class AddProductRequest
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Path { get; set; }
    public string CategoryId { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}