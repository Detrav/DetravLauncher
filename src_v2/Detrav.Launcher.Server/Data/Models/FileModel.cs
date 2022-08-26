using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    public class FileModel : BaseModel
    {
        [Required]
        public string? Path { get; set; }
        public string? Hash { get; set; }
        public long Size { get; set; }
        public virtual ICollection<FileBlobModel> Blobs { get; set; } = new List<FileBlobModel>();
    }
}
