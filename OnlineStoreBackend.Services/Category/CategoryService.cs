using Microsoft.Extensions.Logging;
using OnlineStoreBackend.Abstractions.Models;
using OnlineStoreBackend.Abstractions.Models.Category;
using OnlineStoreBackend.Abstractions.Services.Category;
using OnlineStoreBackend.Abstractions.Services.Product;
using static OnlineStoreBackend.Abstractions.Models.Result;

namespace OnlineStoreBackend.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository, ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _logger = logger;
    }

    private static void ProcessCategoryFields(CategoryDto dto)
    {
        dto.Name = dto.Name.Trim();
        
        dto.Path = (string.IsNullOrEmpty(dto.Path) ? dto.Name : dto.Path)
            .Replace(' ', '_');
    }

    public async Task<Result<string>> Create(CategoryDto dto, CancellationToken ct)
    {
        try
        {
            ProcessCategoryFields(dto);
            var existsByPath = await _categoryRepository.ExistsByPath(dto.Path, ct);
            if (existsByPath) return Fail<string>($"Category with path '{dto.Path}' already exists");

            var parentExistsOrNull = dto.Parent is null
                                     || await _categoryRepository.ExistsById(dto.Parent, ct);
            if (!parentExistsOrNull) return Fail<string>($"Parent category with id '{dto.Parent}' does not exists");

            dto.UpdatedAt = DateTime.UtcNow;
            var result = await _categoryRepository.Create(dto, ct);
            return Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to create category with id '{Id}'", dto.Id);
            return Fail<string>(ex.Message);
        }
    }

    public async Task<Result<CategoryDto>> Get(string id, CancellationToken ct)
    {
        try
        {
            var result = await _categoryRepository.Get(id, ct);
            return result is null ? Fail<CategoryDto>($"Category with id '{id}' not found") : Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to get category with id '{Id}'", id);
            return Fail<CategoryDto>(ex.Message);
        }
    }

    public async Task<Result> Update(CategoryDto dto, CancellationToken ct)
    {
        try
        {
            var categoryById = await _categoryRepository.Get(dto.Id, ct);
            if (categoryById is null) return Fail($"Category with id '{dto.Id}' not found");
            
            ProcessCategoryFields(dto);
            var categoryExists = dto.Path != categoryById.Path
                                && await _productRepository.ExistsByPath(dto.Path, ct);
            if (categoryExists) return Fail($"Category with path '{dto.Path}' already exists");
            
            var parentExistsOrNull = dto.Parent is null
                                     || await _categoryRepository.ExistsById(dto.Parent, ct);
            if (!parentExistsOrNull) return Fail<string>($"Parent category with id '{dto.Parent}' does not exists");

            dto.UpdatedAt = DateTime.UtcNow;
            await _categoryRepository.Update(dto, ct);
            return Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to update category with id '{Id}'", dto.Id);
            return Fail(ex.Message);
        }
    }

    public async Task<Result> Delete(string id, CancellationToken ct)
    {
        try
        {
            var category = await _categoryRepository.Get(id, ct);
            if (category is null) return Fail("Category not found");

            var productsExist = await _productRepository.ExistsByCategoryId(id, ct);
            if (productsExist) return Fail("Category can not be deleted because it contains products");

            await _categoryRepository.Delete(id, ct);
            return Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to delete category with id '{Id}'", id);
            return Fail(ex.Message);
        }
    }

    public async Task<Result<CategoryDto[]>> GetAll(CancellationToken ct)
    {
        try
        {
            var result = await _categoryRepository.GetAll(ct);
            return Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to get all products");
            return Fail<CategoryDto[]>(ex.Message);
        }
    }
}