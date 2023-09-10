using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Services;
using UrlShortener.Infrastructure;

namespace UrlShortener.Core.Test
{
    public class UrlTest
    {
        [Fact]
        public async Task CreateUrl_WithValidData_CreatesUrl()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var urlService = new UrlService(dbContext);
                var userId = "testUser";
                var originalUrl = "https://example.com";

                // Act
                await urlService.CreateUrl(userId, originalUrl);

                // Assert
                var createdUrl = await dbContext.Urls.FirstOrDefaultAsync(u => u.OriginalURL == originalUrl);
                Assert.NotNull(createdUrl);
            }
        }

        [Fact]
        public async Task DeleteUrl_WithValidId_DeletesUrl()
        {
            // Arrange
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(d => d.Urls.FindAsync(It.IsAny<int>())).ReturnsAsync(new URL());

            var urlService = new UrlService(dbContextMock.Object);
            var id = 1;

            // Act
            await urlService.DeleteUrl(id);

            // Assert
            dbContextMock.Verify(d => d.Urls.Remove(It.IsAny<URL>()), Times.Once);
            dbContextMock.Verify(d => d.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

    }
}