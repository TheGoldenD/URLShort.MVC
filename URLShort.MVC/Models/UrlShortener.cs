using System.ComponentModel.DataAnnotations;

namespace URLShort.MVC.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }
        public string? OriginalUrl { get; set; } = default!;
        public string? ShortCode { get; set; } = default!;

        [MaxLength(8)]
        public string RevokePassword { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
