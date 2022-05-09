using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using OnlineStoreBackend.Migration.Core;
using OnlineStoreBackend.Migration.Core.Elastic;
using OnlineStoreBackend.Migration.Migrations;
using Scrutor;

namespace OnlineStoreBackend.Migration;

public class Program
{
    public static async Task Main(string[] args)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") is null)
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        
        using var scope = CreateServices(args).CreateScope();
        await Migrate(scope.ServiceProvider);
    }

    private static async Task Migrate(IServiceProvider scope)
    {
        var runner = scope.GetRequiredService<IMigrationRunner>();
        await runner.MigrateDown();
        await runner.MigrateUp();
    }

    private static IServiceProvider CreateServices(string[] args)
    {
        var configuration = SetupConfiguration(args);
        var elasticUrl = configuration["elasticConfig:Uri"];
        
        return new ServiceCollection()
            .AddSingleton<IElasticClient>(_ =>
            {
                var settings = new ConnectionSettings(new Uri(elasticUrl));
                return new ElasticClient(settings);
            })
            .AddSingleton<IMigrationRepository, EsMigrationRepository>()
            .AddSingleton<IMigrationRunner, MigrationRunner>()
            // .AddSingleton<IMigration, CreateIndexes>()
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

