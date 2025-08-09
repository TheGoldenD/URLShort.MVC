using Microsoft.AspNetCore.Mvc;
using URLShort.MVC.Data;
using URLShort.MVC.Models;
using URLShort.MVC.Helpers;

namespace URLShort.MVC.Controllers
{
    public class UrlController : Controller
    {
        private readonly UrlDBContext _context;

        public UrlController(UrlDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Shorten(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                ModelState.AddModelError("", "Invalid URL format.");
                return View("Index");
            }

            // Store URL in DB
            var entry = new ShortUrl { OriginalUrl = url };
            _context.ShortUrls.Add(entry);
            await _context.SaveChangesAsync();

            // Generate Base62 short code
            var shortUrl = Base62.GenerateShortLink(entry.Id, Request);
            ViewBag.ShortUrl = shortUrl;

            return View("Index");
        }

        [HttpGet("/u/{code}")]
        public async Task<IActionResult> RedirectToOriginal(string code)
        {
            try
            {
                var id = DecodeBase62(code);
                var entry = await _context.ShortUrls.FindAsync(id);

                if (entry == null)
                    return NotFound();

                return Redirect(entry.OriginalUrl);
            }
            catch
            {
                return BadRequest("Invalid code.");
            }
        }

        // Helper to decode Base62 string to int (reversed logic)
        private int DecodeBase62(string code)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            int result = 0;

            foreach (char c in code)
            {
                result = result * 62 + chars.IndexOf(c);
            }

            return result;
        }
    }
}
