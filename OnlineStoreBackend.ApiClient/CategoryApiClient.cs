using OnlineStoreBackend.Api.Models.Category;

namespace OnlineStoreBackend.ApiClient;

public class CategoryApiClient : ApiClientBase, ICategoryApiClient
{
    public CategoryApiClient()
    {
    }

    public async Task<AddCategoryResponse> Create(AddCategoryRequest request, CancellationToken ct = default)
    {
        var result = await HttpClient.PutAsync($"api/v1/Category/create", request, Formatter, ct);
        return await ProcessResponse<AddCategoryResponse>(result);
    }

    public async Task Update(string id, UpdateCategoryRequest request, CancellationToken ct = default)
    {
        var result = await HttpClient.PostAsync($"api/v1/Category/update/{id}", request, Formatter, ct);
        await ProcessResponse(result);
    }

    public async Task Delete(string id, CancellationToken ct = default)
    {
        var result = await HttpClient.DeleteAsync($"api/v1/Category/delete/{id}", ct);
        await ProcessResponse(result);
    }

    public async Task<GetCategoryResponse> Get(string id, CancellationToken ct = default)
    {
        var result = await HttpClient.GetAsync($"api/v1/Category/{id}", ct);
        return await ProcessResponse<GetCategoryResponse>(result);
    }

    public async Task<GetAllCategoriesResponse> GetAll(CancellationToken ct = default)
    {
        var result = await HttpClient.GetAsync($"api/v1/Category/all", ct);
        return await ProcessResponse<GetAllCategoriesResponse>(result);
    }
}

public interface ICategoryApiClient
{
    Task<AddCategoryResponse> Create(AddCategoryRequest request, CancellationToken ct = default);
    Task Update(string id, UpdateCategoryRequest request, CancellationToken ct = default);
    Task Delete(string id, CancellationToken ct = default);
    Task<GetCategoryResponse> Get(string id, CancellationToken ct = default);
    Task<GetAllCategoriesResponse> GetAll(CancellationToken ct = default);
}