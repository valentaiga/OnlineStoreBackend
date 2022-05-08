namespace OnlineStoreBackend.Abstractions.Models.Search;

public class SearchResult
{
    public ProductsSearchResult Products { get; set; }
    public CategoriesSearchResult Categories { get; set; }
}