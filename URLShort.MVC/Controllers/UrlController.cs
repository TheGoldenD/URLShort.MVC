using Microsoft.AspNetCore.Mvc;
using URLShort.MVC.Data;
using URLShort.MVC.Models;
using URLShort.MVC.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

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
        public IActionResult Shorten()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Revoke()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Shorten(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                ModelState.AddModelError("", "URL is required.");
                return View("Shorten");
            }

            // Store URL in DB
            var entry = new ShortUrl { OriginalUrl = url, CreatedAt = DateTime.UtcNow };
            _context.ShortUrls.Add(entry);
            await _context.SaveChangesAsync();

            entry.ShortCode = EncodeUrl.Encode(entry.Id); 
            await _context.SaveChangesAsync();

            entry.RevokePassword = EncodeUrl.GenerateRevokePassword(8);
            await _context.SaveChangesAsync();

            var shortUrl = $"{Request.Scheme}://{Request.Host}/{entry.ShortCode}";
            ViewBag.ShortUrl = shortUrl; 
            ViewBag.RevokePassword = entry.RevokePassword;

            return View("Shorten");
        }

        [HttpPost]
        public async Task<IActionResult> Revoke(string shortenedUrl, string revokePassword)
        {
            if (string.IsNullOrWhiteSpace(shortenedUrl) || string.IsNullOrWhiteSpace(revokePassword))
            {
                ModelState.AddModelError("", "Both URL and revoke password are required.");
                return View("Revoke");
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}/";
            string shortCodeToCheck = shortenedUrl.Split(baseUrl).Last().TrimEnd('/');
            var entry = await _context.ShortUrls
                .FirstOrDefaultAsync(s => s.ShortCode == shortCodeToCheck && s.RevokePassword == revokePassword);

            if (entry == null)
            {
                ModelState.AddModelError("", "No matching URL and revoke password found.");
                return View("Revoke");
            }

            _context.ShortUrls.Remove(entry);
            await _context.SaveChangesAsync();

            ViewBag.Message = "URL successfully revoked and deleted.";
            return View("Revoke");
        }

        [HttpGet("/{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            try
            {
                var originalUrl = await _context.ShortUrls
                         .Where(s => s.ShortCode == shortCode)
                         .Select(s => s.OriginalUrl)
                         .FirstOrDefaultAsync();

                if (originalUrl.IsNullOrEmpty()) 
                    return NotFound();

                return Redirect(originalUrl);
            }
            catch
            {
                return BadRequest("Invalid code.");
            }
        }

    }
}
