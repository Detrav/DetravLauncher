using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    public class BlobModel : BaseModel
    {
        /// <summary>
        /// Data with 256maxkb
        /// </summary>
        [MaxLength(256 * 1024)]
        public byte[]? Data { get; set; }
        public string? Hash { get; set; }
        /// <summary>
        /// Default Size is 256kb, but can be less if file is less
        /// </summary>
        public int Size { get; set; } = 256 * 1024;

        public virtual ICollection<FileBlobModel> Files { get; set; } = new List<FileBlobModel>();
    }
}