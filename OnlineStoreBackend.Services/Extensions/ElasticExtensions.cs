using Nest;

namespace OnlineStoreBackend.Services.Extensions;

public static class ElasticExtensions
{
    public static void EnsureSuccess(this ResponseBase esResponse)
    {
        if (!esResponse.IsValid)
            throw new Exception("Elastic request failed. " +
                                $"{Environment.NewLine}Response: '{esResponse}' " +
                                $"{Environment.NewLine}Error: '{esResponse.ServerError}'");
    }
}