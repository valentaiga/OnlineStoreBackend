using OnlineStoreBackend.Abstractions.Models;
using OnlineStoreBackend.Abstractions.Models.Product;

namespace OnlineStoreBackend.Abstractions.Services.Product;

public interface IProductService
{
    Task<Result<string>> Create(ProductDto dto, CancellationToken ct);
    Task<Result<ProductDto>> Get(string id, CancellationToken ct);
    Task<Result> Update(ProductDto dto, CancellationToken ct);
    Task<Result> Delete(string id, CancellationToken ct);
    Task<Result<ProductDto[]>> GetAll(CancellationToken ct);
}