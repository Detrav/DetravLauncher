using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detrav.Launcher.Server.Data.Models
{
    [Index(nameof(Seek), nameof(FileId), IsUnique = true)]
    public class FileBlobModel
    {
        public virtual BlobModel? Blob { get; set; }
        public int BlobId { get; set; }
        public virtual FileModel? File { get; set; }
        public int FileId { get; set; }
        public long Seek { get; set; }
    }
}
