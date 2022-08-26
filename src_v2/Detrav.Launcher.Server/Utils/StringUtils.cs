namespace Detrav.Launcher.Server.Utils
{
    public static class StringUtils
    {
        public static string TrimOneLine(string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "";

            var line = str.Split('\n').First();

            // TODO trim ... len

            return line;
        }
    }
}
