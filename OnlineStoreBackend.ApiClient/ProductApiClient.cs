using OnlineStoreBackend.Api.Models.Product;

namespace OnlineStoreBackend.ApiClient;

public class ProductApiClient : ApiClientBase, IProductApiClient
{
    public ProductApiClient()
    {
    }
    
    public async Task<AddProductResponse> Create(AddProductRequest request, CancellationToken ct = default)
    {
        var result = await HttpClient.PutAsync($"api/v1/Product/create", request, Formatter, ct);
        return await ProcessResponse<AddProductResponse>(result);
    }

    public async Task Update(string id, UpdateProductRequest request, CancellationToken ct = default)
    {
        var result = await HttpClient.PostAsync($"api/v1/Product/update/{id}", request, Formatter, ct);
        await ProcessResponse(result);
    }

    public async Task Delete(string id, CancellationToken ct = default)
    {
        var result = await HttpClient.DeleteAsync($"api/v1/Product/delete/{id}", ct);
        await ProcessResponse(result);
    }

    public async Task<GetProductResponse> Get(string id, CancellationToken ct = default)
    {
        var result = await HttpClient.GetAsync($"api/v1/Product/{id}", ct);
        return await ProcessResponse<GetProductResponse>(result);
    }

    public async Task<GetAllProductsResponse> GetAll(CancellationToken ct = default)
    {
        var result = await HttpClient.GetAsync($"api/v1/Product/all", ct);
        return await ProcessResponse<GetAllProductsResponse>(result);
    }
}

public interface IProductApiClient
{
    Task<AddProductResponse> Create(AddProductRequest request, CancellationToken ct = default);
    Task Update(string id, UpdateProductRequest request, CancellationToken ct = default);
    Task Delete(string id, CancellationToken ct = default);
    Task<GetProductResponse> Get(string id, CancellationToken ct = default);
    Task<GetAllProductsResponse> GetAll(CancellationToken ct = default);
}