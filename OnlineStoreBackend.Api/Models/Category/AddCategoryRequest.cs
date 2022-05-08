namespace OnlineStoreBackend.Api.Models.Category;

public class AddCategoryRequest
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Path { get; set; }
    public string Parent { get; set; }
    public string Description { get; set; }
}