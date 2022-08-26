using System.ComponentModel.DataAnnotations;

namespace Detrav.Launcher.Server.Data.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }
        public string? CreatorId { get; set; }
        public string? EditorId { get; set; }

        public void SetUpCreator(string id)
        {
            CreatedAt = DateTime.UtcNow;
            CreatorId = id;
        }

        public void SetUpEditor(string id)
        {
            EditedAt = DateTime.UtcNow;
            EditorId = id;
        }
    }
}
