using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    [Index(nameof(Path), nameof(Collection), IsUnique = true)]
    public class FileModel : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Collection { get; set; } = "None";
        [Required]
        [StringLength(1000)]
        public string? Path { get; set; }
        public long Size { get; set; }
        public virtual ICollection<FileBlobModel> Blobs { get; set; } = new List<FileBlobModel>();
    }
}
