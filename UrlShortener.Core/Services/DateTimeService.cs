using UrlShortener.Core.Interfaces;

namespace UrlShortener.Core.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
