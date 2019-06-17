namespace FunctionalTests.DotNet2019.Api
{
    internal static class Api
    {
        public static class Users
        {
            public static string Post()
            {
                return $"api/users";
            }
        }
    }
}
