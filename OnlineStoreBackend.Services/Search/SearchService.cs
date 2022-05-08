using Microsoft.Extensions.Logging;
using OnlineStoreBackend.Abstractions.Models;
using OnlineStoreBackend.Abstractions.Models.Search;
using OnlineStoreBackend.Abstractions.Services.Category;
using OnlineStoreBackend.Abstractions.Services.Product;
using OnlineStoreBackend.Abstractions.Services.Search;
using static OnlineStoreBackend.Abstractions.Models.Result;

namespace OnlineStoreBackend.Services.Search;

public class SearchService : ISearchService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<SearchService> _logger;

    public SearchService(IProductRepository productRepository, ICategoryRepository categoryRepository, ILogger<SearchService> logger)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<Result<SearchResult>> Search(SearchOptions searchOptions, CancellationToken ct)
    {
        try
        {
            var categories = (searchOptions.Type == SearchType.All || searchOptions.Type == SearchType.Category)
                ? await _categoryRepository.Search(searchOptions.Query, searchOptions.Limit, searchOptions.Offset, ct)
                : null;
            var products = (searchOptions.Type == SearchType.All || searchOptions.Type == SearchType.Product)
                ? await _productRepository.Search(searchOptions.Query, searchOptions.Limit, searchOptions.Offset, ct)
                : null;
            var result = new SearchResult
            {
                Categories = categories,
                Products = products
            };
            return Success(result);   
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to search with params q:'{query}' type:'{type}'", searchOptions.Query, searchOptions.Type);
            return Fail<SearchResult>("Search is unavailable");
        }
    }
}