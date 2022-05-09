namespace OnlineStoreBackend.Migration.Core;

public class MigrationAttribute : Attribute
{
    public string Name { get; }
    public int Version { get; }

    public string GetId()
        => $"{Version}-{Name}";

    public MigrationAttribute(int version, string name = null)
    {
        Version = version;
        Name = name;
    }
}