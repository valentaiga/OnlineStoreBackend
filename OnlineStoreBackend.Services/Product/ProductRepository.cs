using Nest;
using OnlineStoreBackend.Abstractions.Models.Product;
using OnlineStoreBackend.Abstractions.Models.Search;
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
        return result.Source.WithId(result.Id);
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

    public async Task<bool> ExistsByCode(string code, CancellationToken ct)
    {
        var result = await _client.SearchAsync<ProductDto>(x =>
            x.Query(q =>
                    q.Match(m =>
                        m.Field(f => f.Code).Query(code)))
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

    public async Task<bool> Exists(string categoryId, string path, CancellationToken ct)
    {
        var result = await _client.SearchAsync<ProductDto>(x =>
            x.Query(q =>
                q.Match(m =>
                    m.Field(f => f.CategoryId).Query(categoryId))
                && q.Match(m =>
                    m.Field(f => f.Path).Query(path)))
                .Take(0), ct);
        
        result.EnsureSuccess();
        return result.Total > 0;
    }

    public async Task<ProductsSearchResult> Search(string query, int limit, int offset, CancellationToken ct)
    {
        var result = await _client.SearchAsync<ProductDto>(x =>
                x.Query(q =>
                        q.Match(m =>
                            m.Field(f => f.Name).Query(query)))
                    .From(offset)
                    .Size(limit)
            , ct);

        return new ProductsSearchResult
        {
            Products = result.Hits.Select(x => x.Source.WithId(x.Id)).ToArray(),
            Total = result.Total
        };
    }

    public async Task<ProductDto[]> GetAll(CancellationToken ct)
    {
        var result = await _client.SearchAsync<ProductDto>(ct: ct);
        return result.Hits.Select(x => x.Source.WithId(x.Id)).ToArray();
    }
}