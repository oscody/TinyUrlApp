namespace TinyUrlApp.Helpers
{
    public static class UrlHelper
    {

        public static bool IsValidUrl(string url) {

            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
