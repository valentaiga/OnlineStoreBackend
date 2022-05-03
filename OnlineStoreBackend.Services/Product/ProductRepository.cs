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
        var req = new DeleteRequest("some-index", id);
        var result = await _client.DeleteAsync(req, ct);
        result.EnsureSuccess();
    }

    public async Task<bool> Exists(string id, CancellationToken ct)
    {
        var result = await _client.DocumentExistsAsync<ProductDto>(id, ct: ct);
        result.EnsureSuccess();
        return result.Exists;
    }
}