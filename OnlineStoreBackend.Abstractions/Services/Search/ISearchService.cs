using OnlineStoreBackend.Abstractions.Models;
using OnlineStoreBackend.Abstractions.Models.Search;

namespace OnlineStoreBackend.Abstractions.Services.Search;

public interface ISearchService
{
    Task<Result<SearchResult>> Search(SearchOptions searchOptions, CancellationToken ct);
}