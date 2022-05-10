using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using OnlineStoreBackend.Migration.Core;
using OnlineStoreBackend.Migration.Core.Elastic;

namespace OnlineStoreBackend.Migration;

public class Program
{
    public static Task Main(string[] args)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") is null)
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        
        using var scope = CreateServices(args).CreateScope();
        return Migrate(scope.ServiceProvider, args);
    }

    private static async Task Migrate(IServiceProvider scope, string[] args)
    {
        var runner = scope.GetRequiredService<IMigrationRunner>();
        if (args.Contains("-md"))
        {
            await runner.MigrateDown();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        await runner.MigrateUp();
    }

    private static IServiceProvider CreateServices(string[] args)
    {
        var configuration = SetupConfiguration(args);
        var elasticUrl = configuration["elasticConfig:Uri"];
        
        return new ServiceCollection()
            .AddSingleton<IElasticClient>(services =>
            {
                var logger = services.GetRequiredService<ILogger<ElasticClient>>();
                var settings = new ConnectionSettings(new Uri(elasticUrl))
                    .DisableDirectStreaming() // for detailed logs
                    .OnRequestCompleted(details => logger.LogInformation($"Request completed. Info:{details.DebugInformation}"));
                return new ElasticClient(settings);
            })
            .AddSingleton<IMigrationRepository, EsMigrationRepository>()
            .AddSingleton<IMigrationRunner, MigrationRunner>()
            .AddLogging(logBuilder => logBuilder.AddConsole())
            .Scan(scan => scan
                .FromAssemblyOf<IMigration>()
                .AddClasses(classes => classes.AssignableTo<IMigration>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime())
            .BuildServiceProvider();
    }

    private static IConfiguration SetupConfiguration(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env}.json", true)
            .AddCommandLine(args)
            .Build();
    }
}

