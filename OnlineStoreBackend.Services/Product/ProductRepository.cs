using Nest;
using OnlineStoreBackend.Abstractions.Models.Product;
using OnlineStoreBackend.Abstractions.Services.Product;
using OnlineStoreBackend.Services.Extensions;

namespace OnlineStoreBackend.Services.Product;

public class ProductRepository : IProductRepository
{
    private readonly IElasticClient _client;

    public ProductRepository(IElasticClient client)
    {
        _client = client;
    }

    public async Task<string> Create(ProductDto dto, CancellationToken ct)
    {
        var result = await _client.IndexDocumentAsync(dto, ct);
        result.EnsureSuccess();
        return result.Id;
    }

    public async Task<ProductDto> Get(string id, CancellationToken ct)
    {
        var result = await _client.GetAsync<ProductDto>(id, ct: ct);
        return result.Source;
    }

    public async Task Update(ProductDto dto, CancellationToken ct)
    {
        var req = new UpdateRequest<ProductDto, ProductDto>(dto.Id)
        {
            Doc = dto
        };
        var result = await _client.UpdateAsync(req, ct);
        result.EnsureSuccess();
    }

    public async Task Delete(string id, CancellationToken ct)
    {
        var result = await _client.DeleteAsync<ProductDto>(id, ct: ct);
        result.EnsureSuccess();
    }

    public async Task<bool> ExistsById(string id, CancellationToken ct)
    {
        var result = await _client.DocumentExistsAsync<ProductDto>(id, ct: ct);
        return result.Exists;
    }

    public async Task<bool> ExistsByPath(string path, CancellationToken ct)
    {
        var result = await _client.SearchAsync<ProductDto>(x =>
            x.Query(q =>
                    q.Match(m =>
                        m.Field(f => f.Path).Query(path)))
                .Take(0), ct);
        
        result.EnsureSuccess();
        return result.Total > 0;
    }

    public async Task<bool> ExistsByCategoryId(string categoryId, CancellationToken ct)
    {
        var result = await _client.SearchAsync<ProductDto>(x =>
            x.Query(q =>
                    q.Match(m =>
                        m.Field(f => f.CategoryId).Query(categoryId)))
                .Take(0), ct);
        
        result.EnsureSuccess();
        return result.Total > 0;
    }

    public async Task<bool> Exists(string id, string path, CancellationToken ct)
    {
        var result = await _client.SearchAsync<ProductDto>(x =>
            x.Query(q =>
                q.Match(m =>
                    m.Field(f => f.Id).Query(id))
                && q.Match(m =>
                    m.Field(f => f.Path).Query(path)))
                .Take(0), ct);
        
        result.EnsureSuccess();
        return result.Total > 0;
    }
}