using OnlineStoreBackend.Abstractions.Models.Category;

namespace OnlineStoreBackend.Api.Models.Category;

public class Category
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Path { get; set; }
    public string Parent { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Category()
    {
    }

    public Category(CategoryDto dto)
    {
        Name = dto.Name;
        IsActive = dto.IsActive;
        Path = dto.Path;
        Parent = dto.Parent;
        UpdatedAt = dto.UpdatedAt;
    }
}