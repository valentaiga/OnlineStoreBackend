using OnlineStoreBackend.Abstractions.Models.Product;

namespace OnlineStoreBackend.Api.Models.Product;

public class GetProductResponse
{
    public ProductDto Result { get; set; }
}