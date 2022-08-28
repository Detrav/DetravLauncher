using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    public class ScreenshotModel : BaseModel
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FilePath { get; set; }
        [Required]
        public virtual ProductModel? Product { get; set; }
    }
}
