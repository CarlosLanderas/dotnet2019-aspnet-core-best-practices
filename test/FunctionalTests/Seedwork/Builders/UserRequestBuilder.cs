using DotNet2019.Api.Model;

namespace FunctionalTests.Seedwork.Builders
{
    internal class UserRequestBuilder
    {
        string name = "Test Portal";

        public UserRequestBuilder()
        {
        }

        public UserRequestBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public UserRequest Build()
        {
            return new UserRequest
            {
                Name = name
            };
        }
    }
}
