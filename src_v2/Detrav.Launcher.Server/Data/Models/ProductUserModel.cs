using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    [Index(nameof(UserId))]
    public class ProductUserModel : BaseModel
    {
        [Required]
        public virtual IdentityUser? User { get; set; }
        [Required]
        public string? UserId { get; set; }
        public virtual ICollection<ProductUserLibraryModel> Products { get; set; } = new List<ProductUserLibraryModel>();
        public virtual ICollection<AchievementModel> Achievements { get; set; } = new List<AchievementModel>();
    }
}
