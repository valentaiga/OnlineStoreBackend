namespace OnlineStoreBackend.Api.Models;

public class InternalExceptionResponse
{
    public string ErrorMessage { get; set; }
    public string Stacktrace { get; set; }
}