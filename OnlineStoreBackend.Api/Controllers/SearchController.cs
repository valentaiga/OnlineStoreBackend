using Microsoft.AspNetCore.Mvc;
using OnlineStoreBackend.Abstractions.Models.Search;
using OnlineStoreBackend.Abstractions.Services.Search;
using OnlineStoreBackend.Api.Models.Search;

namespace OnlineStoreBackend.Api.Controllers;

[Route("api/v1/[controller]")]
public class SearchController : ApiControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<ActionResult<SearchResult>> Search([FromQuery] SearchRequest request, CancellationToken ct)
    {
        var options = new SearchOptions
        {
            Query = request.Query,
            Limit = request.Limit,
            Offset = request.Offset,
            Type = request.Type
        };
        var result = await _searchService.Search(options, ct);

        return Process(result, x => x);
    }
}