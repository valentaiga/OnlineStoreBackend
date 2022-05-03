namespace OnlineStoreBackend.Abstractions.Configs;

public class ElasticConfig
{
    public string Uri { get; set; }
    public string ProductsIndex { get; set; }
    public string LogsIndex { get; set; }
}