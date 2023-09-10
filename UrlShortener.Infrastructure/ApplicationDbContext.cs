using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>, IApplicationDbContext
    {
        public DbSet<URL> Urls { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "TestDB");
        }
    }
}
