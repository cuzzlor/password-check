using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace PasswordCheck.Tests
{
    public class BreachedPasswordServiceTests : IClassFixture<TestFixture>
    {
        private readonly IBreachedPasswordService _breachedPasswordService;

        public BreachedPasswordServiceTests(TestFixture testFixture)
        {
            _breachedPasswordService = testFixture.Host.Services.GetRequiredService<IBreachedPasswordService>();
        }

        [Theory()]
        [InlineData("Password!")]
        [InlineData("Pa55word!")]
        [InlineData("P@ssword!")]
        [InlineData("P@55word!")]
        public async Task BreachedPasswordsAreFound(string password)
        {
            var breachCount = await _breachedPasswordService.GetBreachCountAsync(password);
            breachCount.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task UniquePasswordsAreNotFound()
        {
            var breachCount = await _breachedPasswordService.GetBreachCountAsync(Guid.NewGuid().ToString());
            breachCount.Should().Be(0);
        }
    }
}
