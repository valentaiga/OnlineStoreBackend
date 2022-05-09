using Xunit;

namespace OnlineStoreBackend.Tests;

[CollectionDefinition(Constants.CollectionDefinition)]
public class CollectionFixtureDefinition : ICollectionFixture<TestFixture>
{
}