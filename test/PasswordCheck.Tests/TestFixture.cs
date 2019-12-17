
using Microsoft.Extensions.DependencyInjection;
using System;
using Hosting = Microsoft.Extensions.Hosting;

namespace PasswordCheck.Tests
{
    public class TestFixture
    {
        public readonly Hosting.IHost Host;

        public TestFixture()
        {
            Host = Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) => services.AddBreachedPasswordService())
                .Build();
        }
    }
}
