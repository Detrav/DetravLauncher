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
        public bool IsBeta { get; set; }
        public bool IsPublished { get; set; }
        [Required]
        public virtual ProductModel? Product { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
