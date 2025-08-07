namespace URLShort.MVC.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = default!;
        public string ShortCode { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
