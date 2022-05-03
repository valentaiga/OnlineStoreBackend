namespace OnlineStoreBackend.Api.Models.Product;

public class Product
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Path { get; set; }
    public string CategoryId { get; set; }
}