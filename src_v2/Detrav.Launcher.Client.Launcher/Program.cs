using System.Diagnostics;
using System.IO.Compression;

namespace Detrav.Launcher.Client.Launcher
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var url = File.ReadAllText("settings.txt");

            var (appFolder, version) = FindLatest("app");

            using HttpClient client = new HttpClient();
            var vers = await client.GetStringAsync(url + "/files/version.txt");

            var currentVersion = int.Parse(vers);
            if (appFolder == null || currentVersion > version)
            {
                appFolder = "app_" + currentVersion;
                var zip = await client.GetByteArrayAsync(url + "/files/app.zip");

                var fullPath = Path.GetFullPath(appFolder);
                if (!Directory.Exists(fullPath)) // :)
                    Directory.CreateDirectory(fullPath);

                using var ms = new MemoryStream(zip);

                using ZipArchive zipArchive = new ZipArchive(ms, ZipArchiveMode.Read);
                zipArchive.ExtractToDirectory(fullPath);
            }

            DeleteOld("app", appFolder);
            var info = new ProcessStartInfo();
            info.WorkingDirectory = Directory.GetCurrentDirectory();
            info.FileName = Path.Combine(Path.GetFullPath(appFolder), "Detrav.Launcher.Client.exe");
            info.ArgumentList.Add(Directory.GetCurrentDirectory());
            info.ArgumentList.Add(url);
            Process.Start(info);
        }

        private static void DeleteOld(string v, string appFolder)
        {
            foreach (var dir in Directory.GetDirectories(Directory.GetCurrentDirectory()))
            {
                var name = Path.GetFileName(dir);
                if (name != appFolder)
                {
                    Directory.Delete(dir, true);
                }
            }
        }

        private static (string?, int) FindLatest(string v)
        {
            int latestVersion = -1;
            string? resultName = null;
            foreach (var dir in Directory.GetDirectories(Directory.GetCurrentDirectory()))
            {
                try
                {
                    var name = Path.GetFileName(dir);
                    if (name.StartsWith(v))
                    {
                        var val = int.Parse(name.Split('_')[1]);
                        if (val > latestVersion)
                        {
                            latestVersion = val;
                            resultName = name;
                        }
                    }
                }
                catch
                {
                    // nothing
                }
            }
            return (resultName, latestVersion);
        }
    }
}