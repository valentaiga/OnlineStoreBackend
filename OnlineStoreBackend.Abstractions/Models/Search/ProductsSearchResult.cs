using OnlineStoreBackend.Abstractions.Models.Product;

namespace OnlineStoreBackend.Abstractions.Models.Search;

public class ProductsSearchResult
{
    public ProductDto[] Products { get; set; }
    public long Total { get; set; }
}