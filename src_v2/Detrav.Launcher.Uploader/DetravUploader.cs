using Detrav.Launcher.Server.ViewModels;
using System.Text.Json;

namespace Detrav.Launcher.Uploader
{
    internal class DetravUploader : IDisposable
    {
        private string appUrl;
        private string version;
        private string apikey;
        private string folderPath;

        private readonly List<string> localFiles = new List<string>();
        private readonly HttpClient client = new HttpClient()
        {
            Timeout = TimeSpan.FromHours(1)
        };
        private bool disposedValue;
        private FilesListVM remoteFiles = new FilesListVM();
        private readonly List<string> toUploadFiles = new List<string>();
        private long sizeToUpload;
        private long uploadComplete;

        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public DetravUploader(string url, string version, string apikey, string folderPath)
        {
            this.appUrl = url;
            this.version = version;
            this.apikey = apikey;
            this.folderPath = folderPath;
        }


        public async Task Step2DownloadRemotelistAsync()
        {
            Program.WriteConsoleDelimeter();
            using var request = new HttpRequestMessage(HttpMethod.Get, appUrl + Program.API_GetListWithInfoGet + "?version=" + Uri.EscapeDataString(version));
            request.Headers.TryAddWithoutValidation("X-ApiKey", apikey);
            using var response = await client.SendAsync(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
            var json = await response.Content.ReadAsStringAsync();
            var remoteFiles = JsonSerializer.Deserialize<FilesListVM>(json, jsonOptions);
            if (remoteFiles != null)
                this.remoteFiles = remoteFiles;
            if (this.remoteFiles.Files.Count > 0)
            {
                Console.WriteLine("Found {0} remote files with {1}", this.remoteFiles.Files.Count, GetHumanReadableSize(this.remoteFiles.Files.SelectMany(m => m.Blobs).Sum(m => m.Size)));
            }
            else
            {
                Console.WriteLine("No found remote files!");
            }
        }

        public async Task Step4UploadFilesAsync()
        {
            DateTime time = DateTime.Now;
            TimeSpan step = TimeSpan.FromSeconds(5);
            long lastSize = 0;

            Program.WriteConsoleDelimeter();
            foreach (var file in toUploadFiles)
            {
                var fullpath = Path.Combine(folderPath, file);
                using var fileInDrive = File.OpenRead(fullpath);

                using var request = new HttpRequestMessage(HttpMethod.Post, appUrl + Program.API_UploadPathPost
                    + "?version=" + Uri.EscapeDataString(version)
                    + "&filePath=" + Uri.EscapeDataString(file)
                    + "&fileSize=" + Uri.EscapeDataString(fileInDrive.Length.ToString()));
                using var progressStream = new ProgressReportingStream(fileInDrive);
                progressStream.OnRead += (e, v) =>
                {
                    uploadComplete += v;
                    lastSize += v;
                    var dt = DateTime.Now - time;
                    if (dt > step)
                    {
                        Console.WriteLine("Upload progress: {0:0.00}% - {1}/s",
                            uploadComplete * 100.0 / sizeToUpload,
                            GetHumanReadableSize(lastSize / dt.TotalSeconds));
                        time += dt;
                        lastSize = 0;
                    }
                };
                request.Content = new StreamContent(progressStream);
                request.Headers.TryAddWithoutValidation("X-ApiKey", apikey);
                using var response = await client.SendAsync(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }

        public Task Step3MakeListsAsync()
        {
            Program.WriteConsoleDelimeter();

            // first make all list
            foreach (var file in localFiles)
            {
                toUploadFiles.Add(file);
            }

            MakeCheckIfHasExtraRemoteFilesThatNotExistsLocaly();
            MakeCheckIfHasRemoteFilesThatCannotContinueToUpload();


            sizeToUpload = 0;

            foreach (var file in toUploadFiles)
            {
                var fullpath = Path.Combine(folderPath, file);
                sizeToUpload += new FileInfo(fullpath).Length;
            }

            Console.WriteLine("Need to upload {0} files with {1}", localFiles.Count, GetHumanReadableSize(sizeToUpload));

            return Task.CompletedTask;
        }

        private void MakeCheckIfHasRemoteFilesThatCannotContinueToUpload()
        {
            bool hasError = false;
            // make list that can upload files
            foreach (var fileVM in remoteFiles.Files)
            {
                if (fileVM.Path != null)
                {
                    toUploadFiles.Remove(fileVM.Path);

                    var fullpath = Path.Combine(folderPath, fileVM.Path);
                    var size = new FileInfo(fullpath).Length;

                    if (size != fileVM.Size)
                    {
                        Console.WriteLine("File `{0}` has wrong size: remote - {1} , local - {2}", fileVM.Path, fileVM.Size, size);
                        hasError = true;
                    }
                    else
                    {
                        using var fileInDrive = File.OpenRead(fullpath);
                        long seek = 0;
                        while (true)
                        {
                            var blobData = ReadBlob(fileInDrive);
                            if (blobData == null)
                            {
                                break;
                            }

                            var md5 = CreateMD5(blobData);

                            if (md5 != fileVM.Blobs.FirstOrDefault(m => m.Seek == seek)?.Hash)
                            {
                                Console.WriteLine("File `{0}` has wrong blob: remote - {1} , local - {2}", fileVM.Path, fileVM.Size, size);
                                hasError = true;
                                break;
                            }

                            seek += blobData.Length;
                        }
                    }
                }
            }

            if (hasError)
            {
                throw new NotSupportedException("Continue upload for file is not supported! Delete files with web interface for continue!");
            }
        }

        public static string CreateMD5(byte[] bytes)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(bytes);
                return Convert.ToHexString(hashBytes);
            }
        }

        private byte[]? ReadBlob(Stream fileInDrive)
        {
            byte[] bytes = new byte[256 * 1024];
            int offset = 0;
            while (true)
            {
                var count = fileInDrive.Read(bytes, offset, bytes.Length - offset);
                if (count == 0)
                {
                    if (offset == 0)
                        return null;
                    else
                        return bytes.Take(offset).ToArray();
                }
                offset += count;
                if (offset == bytes.Length)
                {
                    return bytes;
                }
            }
        }

        private void MakeCheckIfHasExtraRemoteFilesThatNotExistsLocaly()
        {
            // make list for check if has to many remote files
            HashSet<string> tempRemoteFiles = new HashSet<string>();
            foreach (var fileVM in remoteFiles.Files)
            {
                if (fileVM.Path != null)
                    tempRemoteFiles.Add(fileVM.Path);
            }

            foreach (var file in toUploadFiles)
            {
                tempRemoteFiles.Remove(file);
            }

            // thor error if found
            if (tempRemoteFiles.Count > 0)
            {
                foreach (var remoteFile in tempRemoteFiles)
                {
                    Console.WriteLine("Found extra remote file: " + remoteFile);
                }
                throw new NotSupportedException("Delete file is not supported! Create a new version for upload new version");
            }
        }

        public Task Step1MakeFilesAsync()
        {
            Program.WriteConsoleDelimeter();

            long size = 0;
            Scan(folderPath, ref size);

            Console.WriteLine("Found {0} files with {1}", localFiles.Count, GetHumanReadableSize(size));

            return Task.CompletedTask;
        }

        private string GetHumanReadableSize(double size)
        {
            string val = "B";
            if (size > 1024)
            {
                size /= 1024;
                val = "KB";
            }
            if (size > 1024)
            {
                size /= 1024;
                val = "MB";
            }
            if (size > 1024)
            {
                size /= 1024;
                val = "GB";
            }

            return size.ToString("0.##") + val;
        }

        private void Scan(string folderPath, ref long size)
        {
            foreach (var file in Directory.GetFiles(folderPath))
            {
                localFiles.Add(file.Replace('\\', '/').Substring(this.folderPath.Length));
                size += new FileInfo(file).Length;
            }

            foreach (var dir in Directory.GetDirectories(folderPath))
            {
                Scan(dir, ref size);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client.Dispose();
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}