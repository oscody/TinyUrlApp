using TinyUrlApp.Models;

namespace TinyUrlApp.DataAccess
{
    public interface IRepository
    {
        Task<bool> AddUrlAsync(UrlEntry url);
        Task<UrlEntry> GetUrlAsync(string shortUrl);
        Task<bool> DeleteUrlAsync(string shortUrl);
    }
    public class Repository: IRepository
    {
        private readonly Dictionary<string, UrlEntry> _urlMappings = new Dictionary<string, UrlEntry>();

        public Task<bool> AddUrlAsync(UrlEntry url)
        {
            return Task.Run(() =>
            {
                lock (_urlMappings)
                {
                    if (_urlMappings.ContainsKey(url.ShortUrl)) return false;
                    _urlMappings[url.ShortUrl] = url;
                    return true;
                }
            });
        }

        public Task<UrlEntry> GetUrlAsync(string shortUrl)
        {
            return Task.Run(() =>
            {
                lock (_urlMappings)
                {
                    return _urlMappings.TryGetValue(shortUrl, out var urlEntry) ? urlEntry : null;
                }
            });
        }

        public Task<bool> DeleteUrlAsync(string shortUrl)
        {
            return Task.Run(() =>
            {
                lock (_urlMappings)
                {
                    return _urlMappings.Remove(shortUrl);
                }
            });
        }
    }
}
