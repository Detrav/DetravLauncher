using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    public class ProductUserModel : BaseModel
    {
        [Required]
        public virtual IdentityUser? User { get; set; }
        [Required]
        public string? UserId { get; set; }
        public virtual ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
        public virtual ICollection<AchievementModel> Achievements { get; set; } = new List<AchievementModel>();
    }
}
