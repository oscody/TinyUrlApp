using System.Collections.Concurrent;
using TinyUrlApp.DataAccess;
using TinyUrlApp.Helpers;
using TinyUrlApp.Models;

namespace TinyUrlApp.Services
{
    public interface IURLService
    {
        Task<string> CreateShortURLAsync(string longURL, string shortURL = null);
        Task<bool> DeleteShortURLAsync(string shortURL);
        int GetAccessCount(string shortURL);
        Task<string> GetLongURLAsync(string shortURL);
    }

    public class URLService : IURLService
    {
        private ConcurrentDictionary<string, int> accessCounts;
        private readonly IRepository _repository;

        public URLService(IRepository repository)
        {
            _repository = repository;
            accessCounts = new ConcurrentDictionary<string, int>();
        }

        public async Task<string> CreateShortURLAsync(string longUrl, string shortUrl = null)
        {
            if (!UrlHelper.IsValidUrl(longUrl)) {

                throw new ArgumentException("The Url is not valid");
            }

            if (shortUrl == null) {

                shortUrl = GenerateShortURL();
            }

            var urlEntry = new UrlEntry
            {
                ShortUrl = shortUrl,
                LongUrl = longUrl,
                AccessCount = 0,
                CreationDate = DateTime.Now
            };

            if (await _repository.AddUrlAsync(urlEntry)) 
            {
                accessCounts[shortUrl] = 0;
                return shortUrl;
            }

            return null;
        }

        public async Task<string> GetLongURLAsync(string shortURL)
        {
            var urlEntry = await _repository.GetUrlAsync(shortURL);
            if (urlEntry != null)
            {
                accessCounts.AddOrUpdate(shortURL, 1, (key, count) => count + 1);
                return urlEntry.LongUrl;
            }
            return null;
        }

        public async Task<bool> DeleteShortURLAsync(string shortURL)
        {
            return await _repository.DeleteUrlAsync(shortURL);
        }
        public int GetAccessCount(string shortUrl)
        {
            return accessCounts.TryGetValue(shortUrl, out var count) ? count : 0;
        }
        private string GenerateShortURL()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
