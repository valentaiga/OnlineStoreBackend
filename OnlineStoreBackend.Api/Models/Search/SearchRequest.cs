using OnlineStoreBackend.Abstractions.Models.Search;

namespace OnlineStoreBackend.Api.Models.Search;

public class SearchRequest
{
    public string Query { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
    public SearchType Type { get; set; }
}