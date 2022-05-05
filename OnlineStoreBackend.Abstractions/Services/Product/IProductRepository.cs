using OnlineStoreBackend.Abstractions.Models.Product;

namespace OnlineStoreBackend.Abstractions.Services.Product;

public interface IProductRepository
{
    Task<string> Create(ProductDto dto, CancellationToken ct);
    Task<ProductDto> Get(string id, CancellationToken ct);
    Task Update(ProductDto dto, CancellationToken ct);
    Task Delete(string id, CancellationToken ct);
    Task<bool> ExistsById(string id, CancellationToken ct);
    Task<bool> ExistsByPath(string path, CancellationToken ct);
    Task<bool> ExistsByCategoryId(string categoryId, CancellationToken ct);
    Task<bool> Exists(string id, string path, CancellationToken ct);
}