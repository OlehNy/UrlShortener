namespace UrlShortener.Core.Entities
{
    public class URL
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string OriginalURL { get; set; }
        public string ShortenedURL { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
