using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using OnlineStoreBackend.Abstractions.Models;
using OnlineStoreBackend.Abstractions.Models.Product;
using OnlineStoreBackend.Abstractions.Services.Product;
using static OnlineStoreBackend.Abstractions.Models.Result;

namespace OnlineStoreBackend.Services.Product;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    private static void ProcessProductFields(ProductDto dto)
    {
        dto.Name = dto.Name.Trim();
        
        dto.Path = (string.IsNullOrEmpty(dto.Path) ? dto.Name : dto.Path)
            .Replace(' ', '_');
    }

    public async Task<Result<string>> Create(ProductDto dto, CancellationToken ct)
    {
        try
        {
            // todo: check for catalogId existence

            dto.UpdatedAt = DateTime.UtcNow;
            ProcessProductFields(dto);
            var result = await _productRepository.Create(dto, ct);
            return Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to create product with id '{Id}'", dto.Id);
            return Fail<string>(ex.Message);
        }
    }

    public async Task<Result<ProductDto>> Get(string id, CancellationToken ct)
    {
        try
        {
            var result = await _productRepository.Get(id, ct);
            return result is null ? Fail<ProductDto>($"Product with id '{id}' not found") : Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to get product with id '{Id}'", id);
            return Fail<ProductDto>(ex.Message);
        }
    }

    public async Task<Result> Update(ProductDto dto, CancellationToken ct)
    {
        try
        {
            var exists = await _productRepository.Exists(dto.Id, ct);
            if (!exists) return Fail<bool>("Product not found");

            dto.UpdatedAt = DateTime.UtcNow;
            ProcessProductFields(dto);
            await _productRepository.Update(dto, ct);
            return Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to update product with id '{Id}'", dto.Id);
            return Fail<bool>(ex.Message);
        }
    }

    public async Task<Result> Delete(string id, CancellationToken ct)
    {
        try
        {
            var exists = await _productRepository.Exists(id, ct);
            if (!exists) return Fail<bool>("Product not found");

            await _productRepository.Delete(id, ct);
            return Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to delete product with id '{Id}'", id);
            return Fail<bool>(ex.Message);
        }
    }
}