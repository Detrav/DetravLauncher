using Detrav.Launcher.Server.Data;
using Detrav.Launcher.Server.Data.Models;

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

        public static string EmailToName(string? email)
        {
            if (email != null)
            {
                var index = email.IndexOf('@');
                if (index >= 1)
                    email = email.Substring(0, index);
            }
            if (String.IsNullOrEmpty(email))
            {
                return "Anonymous";
            }
            return email;
        }
    }
}
