namespace OnlineStoreBackend.Migration.Core;

public interface IMigration
{
    Task MigrateUp();
    Task MigrateDown();
}