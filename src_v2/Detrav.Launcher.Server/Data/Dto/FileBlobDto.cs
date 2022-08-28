namespace Detrav.Launcher.Server.Data.Dto
{
    public class FileBlobDto
    {
        public int FileId { get; set; }
        public int BlobId { get; set; }
        public string? Hash { get; set; }
        public long Size { get; set; }
        public long Seek { get; set; }
    }
}
