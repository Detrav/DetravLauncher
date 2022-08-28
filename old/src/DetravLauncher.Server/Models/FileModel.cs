namespace DetravLauncher.Server.Models
{
    public class FileModel
    {
        public string RelativePath { get; set; } = "";
        public string Hash { get; set; } = "";
        public long Size { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
