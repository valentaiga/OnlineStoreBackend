using OnlineStoreBackend.Abstractions.Models.Product;
using OnlineStoreBackend.Abstractions.Models.Search;

namespace OnlineStoreBackend.Abstractions.Services.Product;

public interface IProductRepository
{
    Task<string> Create(ProductDto dto, CancellationToken ct);
    Task<ProductDto> Get(string id, CancellationToken ct);
    Task Update(ProductDto dto, CancellationToken ct);
    Task Delete(string id, CancellationToken ct);
    Task<bool> ExistsById(string id, CancellationToken ct);
    Task<bool> ExistsByPath(string path, CancellationToken ct);
    Task<bool> ExistsByCode(string code, CancellationToken ct);
    Task<bool> ExistsByCategoryId(string categoryId, CancellationToken ct);
    Task<bool> Exists(string categoryId, string path, CancellationToken ct);
    Task<ProductsSearchResult> Search(string query, int limit, int offset, CancellationToken ct);
    Task<ProductDto[]> GetAll(CancellationToken ct);
}