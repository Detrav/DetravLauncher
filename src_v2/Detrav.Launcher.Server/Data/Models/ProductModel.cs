using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    public class ProductModel : BaseModel
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsPublished { get; set; }
        public virtual FileModel? Poster { get; set; }
        public virtual ICollection<AchievementModel> Achievements { get; set; } = new List<AchievementModel>();
        public virtual ICollection<ScreenshotModel> Screenshots { get; set; } = new List<ScreenshotModel>();
        public virtual ICollection<ProductVersionModel> Versions { get; set; } = new List<ProductVersionModel>();
        public virtual ICollection<TagModel> Tags { get; set; } = new List<TagModel>();
        public virtual ICollection<ProductUserModel> ProductUsers { get; set; } = new List<ProductUserModel>();
    }
}
