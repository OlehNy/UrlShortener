using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Core.Services
{
    public class UrlService : IUrlService
    {
        private readonly IApplicationDbContext _dbContext;

        public UrlService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateUrl(string userId, string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl)) 
                throw new ArgumentNullException(nameof(originalUrl));

            if (await _dbContext.Urls.FirstOrDefaultAsync(url => url.OriginalURL == originalUrl) != null)
                throw new ArgumentException("Url already exist");

            var shortenerUrl = new URL
            {
                OriginalURL = originalUrl,
                UserId = userId,
                ShortenedURL = GetShortenerUrl(originalUrl),
                CreatedDate = new DateTimeService().Now
            };

            await _dbContext.Urls.AddAsync(shortenerUrl);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task DeleteUrl(int id)
        {
            var url = await _dbContext.Urls.FindAsync(id);

            if (url == null)
                throw new ArgumentNullException(nameof(url));

           _dbContext.Urls.Remove(url);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<IReadOnlyList<URL>> GetAll()
            => await _dbContext.Urls.ToListAsync();

        public URL GetUrl(string shortUrl)
        {
            var url = _dbContext.Urls.FirstOrDefault(url => url.ShortenedURL == shortUrl);

            if (url == null)
                throw new ArgumentNullException(nameof(url));

            return url;
        }

        private string GetShortenerUrl(string originalUrl)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(originalUrl));
                string base64Hash = Convert.ToBase64String(hashBytes);

                string shortUrl = base64Hash.Replace("/", "").Replace("+", "").Substring(0, 8);

                return shortUrl;
            }
        }
    }
}
