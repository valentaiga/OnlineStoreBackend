namespace OnlineStoreBackend.Migration.Core;

public interface IMigrationRunner
{
    Task MigrateUp();
    Task MigrateDown();
}