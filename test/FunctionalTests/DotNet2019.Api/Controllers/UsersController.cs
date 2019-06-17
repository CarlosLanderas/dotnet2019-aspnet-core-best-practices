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
    public class users_api_should
    {
        private readonly ServerFixture Given;

        public users_api_should(ServerFixture fixture)
        {
            Given = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        [ResetDatabase]
        public async Task allow_to_add_users()
        {
            var request = Builders.UserRequest
                .WithName("Test")
                .Build();

            var response = await Given
                .Server
                .CreateRequest(Api.Users.Post())
                .WithJsonBody(request)
                .PostAsync();

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
