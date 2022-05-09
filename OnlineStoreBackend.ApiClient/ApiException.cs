namespace OnlineStoreBackend.ApiClient;

public class ApiException : Exception
{
    public ApiException(string error) : base(error)
    {
    }
}