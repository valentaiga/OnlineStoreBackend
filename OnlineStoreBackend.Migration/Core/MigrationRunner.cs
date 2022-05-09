using System.Reflection;
using OnlineStoreBackend.Migration.Core.Models;

namespace OnlineStoreBackend.Migration.Core;

public class MigrationRunner : IMigrationRunner
{
    private readonly IMigrationRepository _migrationRepository;
    private readonly IEnumerable<IMigration> _migrations;

    public MigrationRunner(IMigrationRepository migrationRepository, IEnumerable<IMigration> migrations)
    {
        _migrationRepository = migrationRepository;
        _migrations = migrations;
    }

    public async Task MigrateUp()
    {
        var appliedMigrations = await _migrationRepository.GetAppliedMigrationIds(OrderBy.Asc);

        foreach (var migration in GetMigrationsInfo(_migrations))
        {
            if (appliedMigrations.Contains(migration.id))
                continue;
            
            await migration.migration.MigrateUp();
            await _migrationRepository.SaveMigration(migration.id);
        }
    }

    public async Task MigrateDown()
    {
        var appliedMigrationIds = await _migrationRepository.GetAppliedMigrationIds(OrderBy.Desc);
        var currentMigrations = GetMigrationsInfo(_migrations);
        
        foreach (var migrationId in appliedMigrationIds)
        {
            var current = currentMigrations.FirstOrDefault(x => x.id == migrationId);
            if (current.migration is null)
                continue;

            await current.migration.MigrateDown();
            await _migrationRepository.DeleteMigration(migrationId);
        }
    }

    private static (IMigration migration, string id)[] GetMigrationsInfo(IEnumerable<IMigration> migrations)
    {
        var result = migrations
            .Select(x => (migration: x, attr: GetAttribute(x)))
            .OrderBy(x => x.attr.Version)
            .Select(x => (x.migration, x.attr.GetId()))
            .ToArray();
        return result;
    }

    private static MigrationAttribute GetAttribute(IMigration migration)
    {
        var type = migration.GetType();
        var attr = type.GetCustomAttribute(typeof(MigrationAttribute)) as MigrationAttribute;
        return attr 
            ?? throw new Exception($"Migration {type.Name} does not contain a {nameof(MigrationAttribute)} attribute");
    }
}