using System.Security.Cryptography;
using System.Text;

namespace URLShort.MVC.Helpers
{
    public static class EncodeUrl
    {
        private const string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string Encode(int id)
        {
            var value = id;
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Only non-negative numbers supported.");
            if (value == 0) return "0";

            var ShortCode = new StringBuilder();
            while (value > 0)
            {
                ShortCode.Insert(0, characters[value % characters.Length]);
                value /= characters.Length;
            }

            return ShortCode.ToString();
        }

        public static string GenerateRevokePassword(int length)
        {
            var revokePW = new StringBuilder(length);
            Random rng = new Random();
            {
                
                for (int i = 0; i < length; i++)
                { 
                    var randomIndex = rng.Next(0, characters.Length);
                    Console.WriteLine(characters[randomIndex]);
                    revokePW.Append(characters[randomIndex]);
                }
            }
            return revokePW.ToString();
        }
    }
}
