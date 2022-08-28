namespace Detrav.Launcher.Uploader
{
    internal class Program
    {
        public const string API_UploadPathPost = "/UploadFile";
        public const string API_GetListWithInfoGet = "/GetListWithInfo";

        static async Task Main(string[] args)
        {
            if (args.Length != 4)
            {
                WriteConsoleDelimeter();
                Console.WriteLine("Usage: " + System.AppDomain.CurrentDomain.FriendlyName + " {Url} {Version} {ApiKey} {FolderPath}");
                Console.WriteLine("       Allows you to upload some folder to the launcher Server!");
                WriteConsoleDelimeter();
            }

            string url = args[0];
            string version = args[1];
            string apikey = args[2];
            string folderPath = Path.GetFullPath(args[3]).Replace('\\', '/').TrimEnd('/') + "/";



            

            using var uploader = new DetravUploader(url, version, apikey, folderPath);
            await uploader.Step1MakeFilesAsync();
            await uploader.Step2DownloadRemotelistAsync();
            await uploader.Step3MakeListsAsync();
            await uploader.Step4UploadFilesAsync();


            WriteConsoleDelimeter();
            Console.WriteLine("Completed with: OK!");
        }

        public static void WriteConsoleDelimeter()
        {
            Console.WriteLine("--------------------");
        }
    }
}