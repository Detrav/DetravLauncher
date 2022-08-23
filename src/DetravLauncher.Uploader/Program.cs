// See https://aka.ms/new-console-template for more information

if (args.Length != 3)
{
    Console.WriteLine("usage: " + System.AppDomain.CurrentDomain.FriendlyName + " host apikey folder_name");
    return;
}

string apikey = args[1];
string directory = Path.GetFullPath(args[2]).TrimEnd('/', '\\');
var directoryTrimLen = directory.Length + 1;

if (!Directory.Exists(directory))
{
    throw new DirectoryNotFoundException(directory);
}

using HttpClient httpclient = new HttpClient();
DetravLauncher.DetravLauncherClient client = new DetravLauncher.DetravLauncherClient(args[0], httpclient);

var filesModel = (await client.ListAsync(args[2])).Files.ToDictionary(m => m.RelativePath, m => m);
var newFiles = new List<string>();


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
    newFiles!.Add(file);
}

(string, long) GetMD5Checksum(string filename)
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


foreach (var file in newFiles)
{
    var (hash, size) = GetMD5Checksum(file);
    string name = file.Substring(directoryTrimLen);
    if (filesModel.TryGetValue(name, out var model))
    {
        if (model.Hash == hash && model.Size == size)
        {
            filesModel.Remove(name);
            Console.WriteLine("OK: " + name);
            continue;
        }
    }

    byte[] buffer = new byte[1024 * 1024];
    using var stream = File.OpenRead(file);
    long offset = 0;

    Console.Write("Upload: " + name);

    while (true)
    {
        int count = stream.Read(buffer, 0, buffer.Length);
        if (count == 0)
            break;

        await client.FilePOSTAsync(apikey, new DetravLauncher.FileUploadModel()
        {
            Data = buffer.Take(count).ToArray(),
            Path = name,
            Seek = offset
        });

        offset += count;

        Console.Write(".");
    }

    Console.WriteLine(" - OK!");
}

foreach (var kv in filesModel)
{
    Console.WriteLine("Delete: " + kv.Key);
    await client.FileDELETEAsync(kv.Key, apikey);
}