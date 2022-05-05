using OnlineStoreBackend.Abstractions.Models.Category;

namespace OnlineStoreBackend.Abstractions.Services.Category;

public interface ICategoryRepository
{
    Task<string> Create(CategoryDto dto, CancellationToken ct);
    Task<CategoryDto> Get(string id, CancellationToken ct);
    Task Update(CategoryDto dto, CancellationToken ct);
    Task Delete(string id, CancellationToken ct);
    Task<bool> ExistsById(string id, CancellationToken ct);
    Task<bool> ExistsByPath(string path, CancellationToken ct);
}