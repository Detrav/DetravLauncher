namespace DetravLauncher.Server.Models
{
    public class FileUploadModel
    {
        public string Path { get; set; } = "";
        public byte[] Data { get; set; } = new byte[0];
        public long Seek { get; set; }
    }
}
