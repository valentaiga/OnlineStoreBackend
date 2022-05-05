using Microsoft.AspNetCore.Mvc;
using OnlineStoreBackend.Abstractions.Models.Product;
using OnlineStoreBackend.Abstractions.Services.Product;
using OnlineStoreBackend.Api.Models.Product;

namespace OnlineStoreBackend.Api.Controllers;

[Route("api/v1/[controller]")]
public class ProductController : ApiControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpPut]
    public async Task<ActionResult<AddProductResponse>> Create(AddProductRequest request, CancellationToken ct)
    {
        var dto = new ProductDto
        {
            Name = request.Name,
            Code = request.Code,
            IsActive = request.IsActive,
            CategoryId = request.CategoryId,
            Path = request.Path
        };
        var result = await _service.Create(dto, ct);
        return Process(result, x => new AddProductResponse { Id = result.Value });
    }

    [HttpPost]
    public async Task<ActionResult> Update(string id, UpdateProductRequest request, CancellationToken ct)
    {
        var dto = new ProductDto
        {
            Id = id,
            Name = request.Name,
            Code = request.Code,
            IsActive = request.IsActive,
            CategoryId = request.CategoryId,
            Path = request.Path
        };
        var result = await _service.Update(dto, ct);
        return Process(result);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(string id, CancellationToken ct)
    {
        var result = await _service.Delete(id, ct);
        return Process(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetProductResponse>> Get(string id, CancellationToken ct)
    {
        var result = await _service.Get(id, ct);
        return Process(result, x => new GetProductResponse { Result = new Product(result.Value) });
    }
}