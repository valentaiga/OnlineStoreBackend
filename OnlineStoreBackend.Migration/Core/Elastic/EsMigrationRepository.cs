using Nest;
using OnlineStoreBackend.Migration.Core.Extensions;
using OnlineStoreBackend.Migration.Core.Models;

namespace OnlineStoreBackend.Migration.Core.Elastic;

public class EsMigrationRepository : IMigrationRepository
{
    private const string MigrationIndexName = "migrations";
    private static D Doc = new();
    
    private readonly IElasticClient _client;

    public EsMigrationRepository(IElasticClient client)
    {
        _client = client;
    }
    
    public async Task<HashSet<string>> GetAppliedMigrationIds(OrderBy versionOrdering)
    {
        await EnsureMigrationIndexExists();
        var req = new SearchRequest<D>(MigrationIndexName);
        var result = await _client.SearchAsync<D>(req);
        result.EnsureSuccess();
        return result.Hits.Select(x => x.Id).ToHashSet();
    }

    public async Task SaveMigration(string id)
    {
        await EnsureMigrationIndexExists();
        var req = new CreateRequest<D>(MigrationIndexName, id)
        {
            Document = Doc
        };
        var result = await _client.CreateAsync(req);
        result.EnsureSuccess();
    }

    public async Task DeleteMigration(string id)
    {
        await EnsureMigrationIndexExists();
        var req = new DeleteRequest<D>(MigrationIndexName, id);
        var result = await _client.DeleteAsync(req);
        result.EnsureSuccess();
    }

    private async Task EnsureMigrationIndexExists()
    {
        var indexExists = await _client.Indices.ExistsAsync(MigrationIndexName);
        
        if (indexExists.Exists)
            return;

        var createIndex = await _client.Indices.CreateAsync(MigrationIndexName);
        createIndex.EnsureSuccess();
    }

    private class D
    {
    }
}