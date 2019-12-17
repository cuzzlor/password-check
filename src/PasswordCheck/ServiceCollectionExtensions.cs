using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;

namespace PasswordCheck
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBreachedPasswordService(this IServiceCollection services)
        {
            services.AddScoped<IBreachedPasswordService, BreachedPasswordService>();
            services.AddHttpClient<IBreachedPasswordService, BreachedPasswordService>(client =>
            {
                client.BaseAddress = new Uri("https://api.pwnedpasswords.com");
            });
            return services;
        }

        public static IServiceCollection AddForbiddenPasswordService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IForbiddenPasswordService, ForbiddenPasswordService>();
            services.AddOptions<ForbiddenPasswordOptions>().Bind(configuration.GetSection(nameof(ForbiddenPasswordOptions)));
            return services;
        }
    }
}
