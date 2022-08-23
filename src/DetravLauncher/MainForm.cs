using System.Text.Json;

namespace DetravLauncher
{
    public partial class MainForm : Form
    {
        private SettingsFile? settings;

        public MainForm()
        {
            InitializeComponent();
            btnOK.Click += BtnOK_Click;
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private HashSet<string> oldFiles = new HashSet<string>();

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                settings = JsonSerializer.Deserialize<SettingsFile>(File.ReadAllText("settings.json"));

                if (settings == null)
                {
                    throw new FileNotFoundException("settings.json");
                }

                using HttpClient httpClient = new HttpClient();
                DetravLauncherClient client = new DetravLauncherClient(settings.Host, httpClient);

                var files = await client.ListAsync(settings.FolderName);


                if (files == null)
                    throw new ArgumentNullException("Can't download list of files");

                textBox1.AppendText("Download data for " + files.Files.Count + " files!" + Environment.NewLine);

                Scan(settings.FolderName);

                int i = 0;
                foreach (var file in files.Files)
                {
                    await ScanAsync(file, httpClient, settings.Host);
                    i++;
                    progressBar1.Value = Math.Clamp(i * 100 / files.Files.Count, 0, 100);
                }

                var trimSize = Path.GetFullPath(settings.FolderName).TrimEnd('\\', '/').Length + 1;

                foreach (var file in oldFiles)
                {
                    File.Delete(file);
                    textBox1.AppendText("Delete: " + file.Substring(trimSize) + Environment.NewLine);
                }

                progressBar1.Value = 100;
                BackColor = Color.DarkGreen;
            }
            catch (Exception ex)
            {
                textBox1.AppendText(ex.Message + Environment.NewLine);
                BackColor = Color.DarkRed;
            }
            finally
            {
                btnOK.Text = "Close";
            }
        }

        void Scan(string folder)
        {
            foreach (var file in Directory.GetFiles(folder))
            {
                ScanFile(file);
            }

            foreach (var ch in Directory.GetDirectories(folder))
            {
                Scan(ch);
            }
        }

        void ScanFile(string file)
        {
            oldFiles.Add(Path.GetFullPath(file));
        }

        private async Task ScanAsync(FileModel file, HttpClient client, string host)
        {
            Application.DoEvents();
            var fullPath = Path.GetFullPath(file.RelativePath);

            oldFiles.Remove(fullPath);

            if (File.Exists(fullPath))
            {
                var (hash, size) = GetMD5Checksum(fullPath);

                if (hash == file.Hash && size == file.Size)
                {
                    textBox1.AppendText("OK: " + file.RelativePath + Environment.NewLine);
                }
            }
            Application.DoEvents();
            CheckDir(fullPath);
            Application.DoEvents();

            textBox1.AppendText("Download: " + file.RelativePath + Environment.NewLine);
            var bytes = await client.GetByteArrayAsync(host + "/update/file?path=" + Uri.EscapeDataString(file.RelativePath));

            Application.DoEvents();
            File.WriteAllBytes(fullPath, bytes);
        }

        private static void CheckDir(string fullPath)
        {
            var dir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);
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
    }
}