namespace TinyUrlApp.Models
{
    public class UrlEntry
    {
        public string ShortUrl { get; set; }

        public string LongUrl { get; set; }

        public int AccessCount { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
