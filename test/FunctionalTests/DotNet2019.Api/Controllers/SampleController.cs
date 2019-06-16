using FluentAssertions;
using FunctionalTests.Seedwork;
using FunctionalTests.Seedwork.Builders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.DotNet2019.Api.Controllers
{
    [Collection(nameof(ServerFixtureCollection))]
    [ExcludeFromCodeCoverage]
    public class sample_api_should
    {
        private readonly ServerFixture Given;

        public sample_api_should(ServerFixture fixture)
        {
            Given = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task allow_to_add_samples()
        {
            var request = Builders.Sample
                .WithName("Test")
                .Build();

            var response = await Given
                .Server
                .CreateRequest(Api.Portals.Post())
                .WithJsonBody(request)
                .PostAsync();

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
