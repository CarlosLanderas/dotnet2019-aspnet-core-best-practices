namespace FunctionalTests.Seedwork.Builders
{
    internal static class Builders
    {
        public static UserRequestBuilder UserRequest
        {
            get
            {
                return new UserRequestBuilder();
            }
        }
    }
}
