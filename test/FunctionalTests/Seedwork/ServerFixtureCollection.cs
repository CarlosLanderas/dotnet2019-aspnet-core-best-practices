using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FunctionalTests.Seedwork
{
    [ExcludeFromCodeCoverage]
    [CollectionDefinition(nameof(ServerFixtureCollection))]
    public class ServerFixtureCollection
        :ICollectionFixture<ServerFixture>
    {
    }
}
