using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class TagModel : BaseModel
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public virtual ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}
