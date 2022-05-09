using System.Net;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OnlineStoreBackend.Api.Models;

namespace OnlineStoreBackend.ApiClient;

public class ApiClientBase : IDisposable
{
    protected HttpClient HttpClient;
    protected static readonly JsonMediaTypeFormatter Formatter;

    public void SetHttpClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    protected async Task ProcessResponse(HttpResponseMessage response)
    {
        try
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.InternalServerError:
                {
                    var error = await response.Content.ReadAsAsync<InternalExceptionResponse>();
                    throw new ApiException(ErrorMessage(response, error.Error, error.Stacktrace));
                }
                case HttpStatusCode.BadRequest:
                {
                    var error = await response.Content.ReadAsAsync<BadRequestResponse>();
                    throw new ApiException(ErrorMessage(response, error.Error));
                }
                case HttpStatusCode.Accepted:
                default:
                {
                    response.EnsureSuccessStatusCode();
                    break;
                }
            }
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(ErrorMessage(response, ex.Message, ex.StackTrace));
        }
    }

    protected async Task<TResponse> ProcessResponse<TResponse>(HttpResponseMessage response)
        where TResponse : class
    {
        try
        {
            await ProcessResponse(response);
            return await response.Content.ReadAsAsync<TResponse>()
                   ?? throw new ApiException(ErrorMessage(response));
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(ErrorMessage(response, ex.Message, ex.StackTrace));
        }
    }

    private static string ErrorMessage(HttpResponseMessage response, string errorMessage = null, string stacktrace = null)
        => $"{response.StatusCode}: '{response.RequestMessage.RequestUri}' Reason:'{response.ReasonPhrase}' " +
           $"{Environment.NewLine}{errorMessage}" + 
           $"{Environment.NewLine}{stacktrace}";
    
    static ApiClientBase()
    {
        Formatter = new JsonMediaTypeFormatter
        {
            SerializerSettings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                },
                NullValueHandling = NullValueHandling.Include,
            }
        };
    }

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}