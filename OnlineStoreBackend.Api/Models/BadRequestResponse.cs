namespace OnlineStoreBackend.Api.Models;

public class BadRequestResponse
{
    public string Error { get; set; }

    public BadRequestResponse(string error)
    {
        Error = error;
    }
}