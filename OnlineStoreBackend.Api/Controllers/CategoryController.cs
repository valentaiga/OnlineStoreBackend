using Microsoft.AspNetCore.Mvc;
using OnlineStoreBackend.Abstractions.Models.Category;
using OnlineStoreBackend.Abstractions.Services.Category;
using OnlineStoreBackend.Api.Models.Category;

namespace OnlineStoreBackend.Api.Controllers;

[Route("api/v1/[controller]")]
public class CategoryController : ApiControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpPut("create")]
    public async Task<ActionResult<AddCategoryResponse>> Create(AddCategoryRequest request, CancellationToken ct)
    {
        var dto = new CategoryDto
        {
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive,
            Parent = request.Parent,
            Path = request.Path
        };
        var result = await _service.Create(dto, ct);
        return Process(result, x => new AddCategoryResponse { Id = result.Value });
    }

    [HttpPost("update/{id}")]
    public async Task<ActionResult> Update(string id, UpdateCategoryRequest request, CancellationToken ct)
    {
        var dto = new CategoryDto
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive,
            Parent = request.Parent,
            Path = request.Path
        };
        var result = await _service.Update(dto, ct);
        return Process(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken ct)
    {
        var result = await _service.Delete(id, ct);
        return Process(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetCategoryResponse>> Get(string id, CancellationToken ct)
    {
        var result = await _service.Get(id, ct);
        return Process(result, x => new GetCategoryResponse { Result = new Category(result.Value) });
    }

    [HttpGet("all")]
    public async Task<ActionResult<GetAllCategoriesResponse>> GetAll(CancellationToken ct)
    {
        var result = await _service.GetAll(ct);
        return Process(result, x => new GetAllCategoriesResponse { Result = x.Value.Select(x => new Category(x)).ToArray() });
    }
}