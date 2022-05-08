using OnlineStoreBackend.Abstractions.Models.Category;

namespace OnlineStoreBackend.Api.Models.Category;

public class Category
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public string Path { get; set; }
    public string Parent { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Category()
    {
    }

    public Category(CategoryDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        Description = dto.Description;
        IsActive = dto.IsActive;
        Path = dto.Path;
        Parent = dto.Parent;
        UpdatedAt = dto.UpdatedAt;
    }
}