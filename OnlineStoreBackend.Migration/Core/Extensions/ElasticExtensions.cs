using Nest;

namespace OnlineStoreBackend.Migration.Core.Extensions;

internal static class ElasticExtensions
{
    public static void EnsureSuccess(this IResponse esResponse)
    {
        if (!esResponse.IsValid)
            throw new Exception("Elastic request failed. " +
                                $"{Environment.NewLine}Response: '{esResponse}' " +
                                $"{Environment.NewLine}Error: '{esResponse.ServerError}'");
    }
    
    public static void EnsureSuccess<TDocument>(this ISearchResponse<TDocument> esResponse) 
        where TDocument : class
    {
        if (!esResponse.IsValid)
            throw new Exception("Elastic request failed. " +
                                $"{Environment.NewLine}Response: '{esResponse}' " +
                                $"{Environment.NewLine}Error: '{esResponse.ServerError}'");
    }
}