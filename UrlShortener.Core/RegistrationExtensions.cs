using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Services;

namespace UrlShortener.Core
{
    public static class RegistrationExtensions
    {
        public static void AddCore(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDateTime, DateTimeService>();
            serviceCollection.AddScoped<IUrlService, UrlService>();
        }
    }
}
