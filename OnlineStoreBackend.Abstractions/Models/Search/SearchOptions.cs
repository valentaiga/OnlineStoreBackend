namespace OnlineStoreBackend.Abstractions.Models.Search;

public class SearchOptions
{
    public string Query { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
    public SearchType Type { get; set; }
}