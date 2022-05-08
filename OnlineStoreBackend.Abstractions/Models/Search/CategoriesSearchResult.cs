using OnlineStoreBackend.Abstractions.Models.Category;

namespace OnlineStoreBackend.Abstractions.Models.Search;

public class CategoriesSearchResult
{
    public CategoryDto[] Products { get; set; }
    public long Total { get; set; }
}