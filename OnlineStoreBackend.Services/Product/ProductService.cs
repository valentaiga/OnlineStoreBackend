using Microsoft.Extensions.Logging;
using OnlineStoreBackend.Abstractions.Models;
using OnlineStoreBackend.Abstractions.Models.Product;
using OnlineStoreBackend.Abstractions.Services.Category;
using OnlineStoreBackend.Abstractions.Services.Product;
using static OnlineStoreBackend.Abstractions.Models.Result;

namespace OnlineStoreBackend.Services.Product;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    private static void ProcessProductFields(ProductDto dto)
    {
        dto.Name = dto.Name.Trim();
        
        dto.Path = (string.IsNullOrEmpty(dto.Path) ? dto.Name : dto.Path)
            .Replace(' ', '_');

        dto.Code = dto.Code.Replace(' ', '_');
    }

    public async Task<Result<string>> Create(ProductDto dto, CancellationToken ct)
    {
        try
        {
            ProcessProductFields(dto);
            var existsByPath = await _productRepository.ExistsByPath(dto.Path, ct);
            if (existsByPath)
                return Fail<string>($"Product with path '{dto.Path}' already exists");

            var categoryExists = await _categoryRepository.ExistsById(dto.CategoryId, ct); 
            if (!categoryExists)
                return Fail<string>($"Category with id '{dto.CategoryId}' not found");

            var existsByCode = await _productRepository.ExistsByCode(dto.Code, ct); 
            if (!existsByCode)
                return Fail<string>($"Product with code '{dto.Code}' already exists");

            dto.UpdatedAt = DateTime.UtcNow;
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
            var productById = await _productRepository.Get(dto.Id, ct);
            if (productById is null) return Fail($"Product with id '{dto.Id}' not found");
            
            var existsByPath = dto.Path != productById.Path 
                                && await _productRepository.ExistsByPath(dto.Path, ct);
            if (!existsByPath) return Fail($"Product with path '{dto.Id}' already exists");
            
            var existsByCode = dto.Code != productById.Code
                               && await _productRepository.ExistsByCode(dto.Path, ct);
            if (!existsByCode) return Fail($"Product with code '{dto.Code}' already exists");
            
            var categoryExists = await _categoryRepository.ExistsById(dto.CategoryId, ct); 
            if (!categoryExists)
                return Fail($"Category with id '{dto.Id}' not found");

            dto.UpdatedAt = DateTime.UtcNow;
            ProcessProductFields(dto);
            await _productRepository.Update(dto, ct);
            return Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to update product with id '{Id}'", dto.Id);
            return Fail(ex.Message);
        }
    }

    public async Task<Result> Delete(string id, CancellationToken ct)
    {
        try
        {
            var exists = await _productRepository.ExistsById(id, ct);
            if (!exists) return Fail("Product not found");

            await _productRepository.Delete(id, ct);
            return Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to delete product with id '{Id}'", id);
            return Fail(ex.Message);
        }
    }

    public async Task<Result<ProductDto[]>> GetAll(CancellationToken ct)
    {
        try
        {
            var result = await _productRepository.GetAll(ct);
            return Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to get all products");
            return Fail<ProductDto[]>(ex.Message);
        }
    }
}