using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    public class ProductVersionModel : BaseModel
    {
        [Required]
        public string? Version { get; set; }
        public virtual ICollection<FileModel> Files { get; set; } = new List<FileModel>();

        public long Size { get; set; }

        [Required]
        public virtual ProductModel? Product { get; set; }
    }
}
