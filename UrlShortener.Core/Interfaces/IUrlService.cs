using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Interfaces
{
    public interface IUrlService
    {
        URL GetUrl(string shortUrl);
        Task<IReadOnlyList<URL>> GetAll();
        Task CreateUrl(string userId, string originalUrl);
        Task DeleteUrl(int id);
    }
}
