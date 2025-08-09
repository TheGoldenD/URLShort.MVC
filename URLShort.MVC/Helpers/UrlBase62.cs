using System.Text;

namespace URLShort.MVC.Helpers
{
    public static class Base62
    {
        private const string Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        // Encode a positive integer to Base62
        public static string Encode(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Only non-negative numbers supported.");
            if (value == 0) return "0";

            var sb = new StringBuilder();
            while (value > 0)
            {
                sb.Insert(0, Characters[value % 62]);
                value /= 62;
            }

            return sb.ToString();
        }

        // Optional: Generate full short URL with request context
        public static string GenerateShortLink(int id, HttpRequest request)
        {
            var code = Encode(id);
            return $"{request.Scheme}://{request.Host}/u/{code}";
        }
    }
}
