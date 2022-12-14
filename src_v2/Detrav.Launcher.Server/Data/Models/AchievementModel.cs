using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    public class AchievementModel : BaseModel
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? IconFilePath { get; set; }
        public bool IsHidden { get; set; }
        [Required]
        public virtual ProductModel? Product { get; set; }
        public virtual ICollection<ProductUserModel> ProductUsers { get; set; } = new List<ProductUserModel>();
    }
}
