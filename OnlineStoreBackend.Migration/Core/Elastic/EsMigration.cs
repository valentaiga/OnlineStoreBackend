using Nest;

namespace OnlineStoreBackend.Migration.Core.Elastic;

public abstract class EsMigration : IMigration
{
    protected IElasticClient Client { get; }

    protected EsMigration(IElasticClient client)
    {
        Client = client;
    }
    
    public abstract Task MigrateUp();
    public abstract Task MigrateDown();
}