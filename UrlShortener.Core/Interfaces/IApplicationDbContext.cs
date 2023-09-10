using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<URL> Urls { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
