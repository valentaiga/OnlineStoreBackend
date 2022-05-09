using OnlineStoreBackend.Migration.Core.Models;

namespace OnlineStoreBackend.Migration.Core;

public interface IMigrationRepository
{
    Task<HashSet<string>> GetAppliedMigrationIds(OrderBy versionOrdering);
    Task SaveMigration(string id);
    Task DeleteMigration(string id);
}