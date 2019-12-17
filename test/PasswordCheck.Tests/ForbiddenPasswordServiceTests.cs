using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace PasswordCheck.Tests
{
    public class ForbiddenPasswordServiceTests : IClassFixture<TestFixture>
    {
        private readonly IForbiddenPasswordService _forbiddenPasswordService;

        public ForbiddenPasswordServiceTests(TestFixture testFixture)
        {
            _forbiddenPasswordService = testFixture.Host.Services.GetRequiredService<IForbiddenPasswordService>();
        }

        [Theory()]
        [InlineData("Password!")]
        [InlineData("Pa55word!")]
        [InlineData("P@ssword!")]
        [InlineData("P@55word!")]
        public void ForbiddenPasswordsAreLoadedFromFile(string password)
        {
            _forbiddenPasswordService.IsPasswordForbidden(password).Should().BeTrue();
        }

        [Theory()]
        [InlineData("forbidden")]
        [InlineData("Forbidden")]
        [InlineData("F0rb1dd3n")]
        [InlineData("F0rb1dd3n!123")]
        public void ForbiddenPasswordsAreLoadedFromConfig(string password)
        {
            _forbiddenPasswordService.IsPasswordForbidden(password).Should().BeTrue();
        }
    }
}
