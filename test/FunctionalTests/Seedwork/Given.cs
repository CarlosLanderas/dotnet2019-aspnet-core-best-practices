using System.Diagnostics.CodeAnalysis;

namespace FunctionalTests.Seedwork
{
    [ExcludeFromCodeCoverage]
    public class Given
    {
        private readonly ServerFixture _serverFixture;

        public Given(ServerFixture serverFixture)
        {
            _serverFixture = serverFixture;
        }
    }
}
