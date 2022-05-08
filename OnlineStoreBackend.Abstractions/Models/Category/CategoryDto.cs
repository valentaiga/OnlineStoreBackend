namespace OnlineStoreBackend.Abstractions.Models.Category;

public class CategoryDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public string Path { get; set; }
    public string Parent { get; set; }
    public DateTime UpdatedAt { get; set; }

    public CategoryDto()
    {
    }

    public CategoryDto WithId(string id)
    {
        Id = id;
        return this;
    }
}