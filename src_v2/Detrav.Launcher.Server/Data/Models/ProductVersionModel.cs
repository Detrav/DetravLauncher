using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    [Index(nameof(ProductVersionModel.Version))]
    public class ProductVersionModel : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string? Version { get; set; }
        public virtual ICollection<ProductVersionFileModel> Files { get; set; } = new List<ProductVersionFileModel>();
        public bool IsBeta { get; set; }
        public bool IsPublished { get; set; }

        public long Size { get; set; }

        [Required]
        public virtual ProductModel? Product { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
