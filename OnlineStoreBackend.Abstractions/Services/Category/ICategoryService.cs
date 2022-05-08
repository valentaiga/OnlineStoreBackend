using OnlineStoreBackend.Abstractions.Models;
using OnlineStoreBackend.Abstractions.Models.Category;

namespace OnlineStoreBackend.Abstractions.Services.Category;

public interface ICategoryService
{
    Task<Result<string>> Create(CategoryDto dto, CancellationToken ct);
    Task<Result<CategoryDto>> Get(string id, CancellationToken ct);
    Task<Result> Update(CategoryDto dto, CancellationToken ct);
    Task<Result> Delete(string id, CancellationToken ct);
    Task<Result<CategoryDto[]>> GetAll(CancellationToken ct);
}