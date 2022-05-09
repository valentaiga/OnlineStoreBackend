using Nest;
using OnlineStoreBackend.Migration.Core;
using OnlineStoreBackend.Migration.Core.Elastic;

namespace OnlineStoreBackend.Migration.Migrations;

[Migration(1)]
public class CreateIndexes : EsMigration
{
    public CreateIndexes(IElasticClient client) : base(client)
    {
    }
    
    public override async Task MigrateUp()
    {
        await Client.Indices.CreateAsync("products");
        await Client.Indices.CreateAsync("categories");
    }

    public override async Task MigrateDown()
    {
        await Client.Indices.DeleteAsync("products");
        await Client.Indices.DeleteAsync("categories");
    }
}