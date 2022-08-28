namespace Detrav.Launcher.Server.ViewModels
{
    public class FileVM
    {
        public long Size { get; set; }
        public string? Path { get; set; }
        public List<FileBlobVM> Blobs { get; set; } = new List<FileBlobVM>();
    }
}
