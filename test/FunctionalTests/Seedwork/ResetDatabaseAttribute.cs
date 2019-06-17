using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Xunit.Sdk;

namespace FunctionalTests.Seedwork
{
    [ExcludeFromCodeCoverage]
    public class ResetDatabaseAttribute
        : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            ServerFixture.ResetDatabase();
        }
    }
}
