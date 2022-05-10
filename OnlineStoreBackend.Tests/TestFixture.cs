using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using OnlineStoreBackend.ApiClient;

namespace OnlineStoreBackend.Tests;

public class TestFixture
{
    private static TestServer Server { get; set; }
    public ICategoryApiClient CategoryApiClient { get; }
    public IProductApiClient ProductApiClient { get; }

    public TestFixture()
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))) 
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        var builder = new WebHostBuilder()
            .ConfigureAppConfiguration((_, builder) =>
            {
                builder
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile(
                        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                        true);
            })
            .UseStartup<Startup>();
        Server = new TestServer(builder);

        CategoryApiClient = CreateApiClient<CategoryApiClient>();
        ProductApiClient = CreateApiClient<ProductApiClient>();

        Migration.Program.Main(new[] { "-md" }).GetAwaiter().GetResult();
    }

    private TApiClient CreateApiClient<TApiClient>() where TApiClient : ApiClientBase, new()
    {
        var httpClient = Server.CreateClient();
        httpClient.BaseAddress = new Uri("http://localhost:5000");
        var apiClient = new TApiClient();
        apiClient.SetHttpClient(httpClient);
        return apiClient;
    }
}