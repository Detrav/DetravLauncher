namespace Detrav.Launcher.Server.Utils
{
    public static class AppUtils
    {
        public static string TrimOneLine(string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "";

            var line = str.Split('\n').First();

            // TODO trim ... len

            return line;
        }

        public static string AbsoluteUrlPrefix(HttpRequest request, string path)
        {
            return request.Scheme + "://" + request.Host + request.PathBase + path;
        }
    }
}
