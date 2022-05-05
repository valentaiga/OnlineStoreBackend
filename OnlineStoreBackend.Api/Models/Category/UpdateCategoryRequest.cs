namespace OnlineStoreBackend.Api.Models.Category;

public class UpdateCategoryRequest
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Path { get; set; }
    public string Parent { get; set; }
}