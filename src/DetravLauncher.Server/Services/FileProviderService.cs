using DetravLauncher.Server.Models;
using Microsoft.Extensions.Options;

namespace DetravLauncher.Server.Services
{
    public class FileProviderService
    {
        private readonly MainConfigModel config;
        private readonly string directory;
        private readonly int directoryTrimLen;
        private readonly object _locker = new object();
        private readonly Dictionary<string, FileModel> files = new Dictionary<string, FileModel>();

        public FileProviderService(IOptions<MainConfigModel> config)
        {
            this.config = config.Value;

            if (String.IsNullOrWhiteSpace(this.config.ContentPath))
                throw new ArgumentNullException(nameof(this.config.ContentPath));

            if (!Directory.Exists(this.config.ContentPath))
                Directory.CreateDirectory(this.config.ContentPath);


            var directory = Path.GetFullPath(this.config.ContentPath);

            this.directory = directory.TrimEnd('/', '\\');
            this.directoryTrimLen = directory.Length + 1;
        }

        public IEnumerable<FileModel> GetList(string name)
        {
            string folder = GetFileInternal(name);

            if (Directory.Exists(folder))

            {
                lock (_locker)
                {

                    foreach (var item in Scan(folder))
                    {
                        yield return item;
                    }
                }
            }
        }

        private IEnumerable<FileModel> Scan(string folder)
        {
            foreach (var file in Directory.GetFiles(folder))
            {
                yield return ScanFile(file);
            }

            foreach (var ch in Directory.GetDirectories(folder))
            {
                foreach (var file in Scan(ch))
                {
                    yield return file;
                }
            }
        }

        private FileModel ScanFile(string file)
        {
            string name = file.Substring(directoryTrimLen);

            if (files.TryGetValue(name, out var fileModel))
            {
                if (File.GetCreationTime(file) == fileModel.CreatedAt)
                {
                    return fileModel;
                }
            }


            fileModel = new FileModel();

            fileModel.RelativePath = name;
            (fileModel.Hash, fileModel.Size) = GetMD5Checksum(file);
            fileModel.CreatedAt = File.GetCreationTime(file);


            files[name] = fileModel;


            return fileModel;
        }

        public static (string, long) GetMD5Checksum(string filename)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return (BitConverter.ToString(hash).Replace("-", ""), stream.Length);
                }
            }
        }

        private string GetFileInternal(string name)
        {
            var file = Path.Combine(directory, name);

            file = Path.GetFullPath(file);

            if (!file.StartsWith(directory))
                throw new FileNotFoundException(name);

            return file;
        }

        public string GetFile(string path)
        {
            return GetFileInternal(path);
        }
    }
}
