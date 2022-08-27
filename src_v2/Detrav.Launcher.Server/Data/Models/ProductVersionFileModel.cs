using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    public class ProductVersionFileModel
    {
        public int VersionId { get; set; }
        public virtual ProductVersionModel? Version { get; set; }
        public int FileId { get; set; }
        public virtual FileModel? File { get; set; }
    }
}
