using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;


namespace UrlShortener.Infrastructure
{
    public static class RegistrationExtensions
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDB"));

            serviceCollection.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        }
    }
}
