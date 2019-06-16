using DotNet2019.Api.Model;

namespace FunctionalTests.Seedwork.Builders
{
    internal class SampleBuilder
    {
        string name = "Test Portal";
        int id = 1;

        public SampleBuilder()
        {
        }

        public SampleBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public SampleRequest Build()
        {
            return new SampleRequest
            {
                Id = id,
                Name = name
            };
        }
    }
}
