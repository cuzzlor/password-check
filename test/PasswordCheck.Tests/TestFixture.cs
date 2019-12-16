
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
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IBreachedPasswordService, BreachedPasswordService>();
                    services.AddHttpClient("hibp-range", client =>
                    {
                        client.BaseAddress = new Uri("https://api.pwnedpasswords.com");
                    });
                }).Build();
        }
    }
}
