using OnlineStoreBackend.Abstractions.Models.Product;

namespace OnlineStoreBackend.Abstractions.Services.Product;

public interface IProductRepository
{
    Task<string> Create(ProductDto dto, CancellationToken ct);
    Task<ProductDto> Get(string id, CancellationToken ct);
    Task Update(ProductDto dto, CancellationToken ct);
    Task Delete(string id, CancellationToken ct);
    Task<bool> Exists(string id, CancellationToken ct);
}