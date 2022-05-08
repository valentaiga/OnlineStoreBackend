using Nest;
using OnlineStoreBackend.Abstractions.Models.Category;
using OnlineStoreBackend.Abstractions.Models.Search;
using OnlineStoreBackend.Abstractions.Services.Category;
using OnlineStoreBackend.Services.Extensions;

namespace OnlineStoreBackend.Services.Category;

public class CategoryRepository : ICategoryRepository
{
    private readonly IElasticClient _client;

    public CategoryRepository(IElasticClient client)
    {
        _client = client;
    }

    public async Task<string> Create(CategoryDto dto, CancellationToken ct)
    {
        var result = await _client.IndexDocumentAsync(dto, ct);
        result.EnsureSuccess();
        return result.Id;
    }

    public async Task<CategoryDto> Get(string id, CancellationToken ct)
    {
        var result = await _client.GetAsync<CategoryDto>(id, ct: ct);
        return result.Source.WithId(result.Id);
    }

    public async Task Update(CategoryDto dto, CancellationToken ct)
    {
        var req = new UpdateRequest<CategoryDto, CategoryDto>(dto.Id)
        {
            Doc = dto
        };
        var result = await _client.UpdateAsync(req, ct);
        result.EnsureSuccess();
    }

    public async Task Delete(string id, CancellationToken ct)
    {
        var result = await _client.DeleteAsync<CategoryDto>(id, ct: ct);
        result.EnsureSuccess();
    }

    public async Task<bool> ExistsById(string id, CancellationToken ct)
    {
        var result = await _client.DocumentExistsAsync<CategoryDto>(id, ct: ct);
        return result.Exists;
    }

    public async Task<bool> ExistsByPath(string path, CancellationToken ct)
    {
        var result = await _client.SearchAsync<CategoryDto>(x =>
            x.Query(q =>
                    q.Match(m =>
                        m.Field(f => f.Path).Query(path)))
                .Take(0), ct);
        
        result.EnsureSuccess();
        return result.Total > 0;
    }

    public async Task<CategoriesSearchResult> Search(string query, int limit, int offset, CancellationToken ct)
    {
        var result = await _client.SearchAsync<CategoryDto>(x =>
            x.Query(q =>
                    q.Match(m =>
                        m.Field(f => f.Name).Query(query)))
                .From(offset)
                .Size(limit)
            , ct);
        
        return new CategoriesSearchResult
        {
            Products = result.Hits.Select(x => x.Source.WithId(x.Id)).ToArray(),
            Total = result.Total
        };
    }

    public async Task<CategoryDto[]> GetAll(CancellationToken ct)
    {
        var result = await _client.SearchAsync<CategoryDto>(ct: ct);
        return result.Hits.Select(x => x.Source.WithId(x.Id)).ToArray();
    }
}